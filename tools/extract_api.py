"""Reads the public C# scripting API out of the engine sources.

    python extract_api.py [engine_root]  ->  tools/api_raw.json

Walks ElintriaSharp/*.cs and Packages/*/csharp/*.cs, finds every public
class, struct, enum and interface, and every public member inside it, along
with the comment lines written just above each declaration. The hand-written
documentation in tools/content/ is merged on top of this by build_docs.py.
"""
import os, re, sys, json

HERE = os.path.dirname(os.path.abspath(__file__))
DEFAULT_ROOT = os.path.normpath(os.path.join(HERE, "..", "..", "Engine", "ElintriaGameEngine"))

TYPE_RE = re.compile(
    r'^\s*(?:\[[^\]]*\]\s*)*public\s+((?:static|sealed|abstract|partial|readonly|unsafe|new)\s+)*'
    r'(class|struct|enum|interface)\s+(\w+)(\s*<[^>]+>)?\s*(?::\s*([^{/]+))?')
MEMBER_START = re.compile(r'^\s*(?:\[[^\]]*\]\s*)*public\s')
PROTECTED_START = re.compile(r'^\s*protected\s+(abstract|virtual)\s')
KEYWORDS = r'(?:static|virtual|override|abstract|sealed|readonly|const|new|extern|unsafe|async|event|implicit|explicit|operator|volatile)'


def strip_code(line, state):
    """The line with strings, chars and comments blanked, so braces can be counted.
    state['block'] carries an open /* comment across lines."""
    out, i, n = [], 0, len(line)
    while i < n:
        if state["block"]:
            j = line.find("*/", i)
            if j < 0: return "".join(out)
            state["block"] = False; i = j + 2; continue
        ch = line[i]
        if line.startswith("//", i): break
        if line.startswith("/*", i): state["block"] = True; i += 2; continue
        if ch == '"' or (ch == '@' and i + 1 < n and line[i + 1] == '"') or (ch == '$' and i + 1 < n and line[i + 1] == '"'):
            verbatim = ch == '@'
            i += 2 if ch in '@$' else 1
            while i < n:
                if not verbatim and line[i] == '\\': i += 2; continue
                if line[i] == '"':
                    if verbatim and i + 1 < n and line[i + 1] == '"': i += 2; continue
                    i += 1; break
                i += 1
            out.append('""'); continue
        if ch == "'":
            j = i + 1
            while j < n and line[j] != "'":
                j += 2 if line[j] == '\\' else 1
            out.append("' '"); i = j + 1; continue
        out.append(ch); i += 1
    return "".join(out)


def clean_comment(lines):
    text = []
    for l in lines:
        s = l.strip()
        s = re.sub(r'^///?\s?', '', s)
        s = re.sub(r'</?(summary|remarks|para|c|returns)>', '', s)
        s = re.sub(r'<param name="(\w+)">', r'\1: ', s).replace('</param>', '')
        s = re.sub(r'<see cref="([^"]+)"\s*/>', r'\1', s)
        if re.match(r'^[─━═\-=]{6,}', s): continue
        s = s.strip('─ ').strip()
        text.append(s)
    out = " ".join(t for t in text if t)
    return re.sub(r'\s+', ' ', out).strip()


def split_body(text):
    """(head, body): head is the declaration up to its body or initializer."""
    depth = 0
    for i, ch in enumerate(text):
        if ch in '(<[': depth += 1
        elif ch in ')>]': depth -= 1
        elif depth <= 0:
            if ch == '{': return text[:i].strip(), text[i:]
            if text.startswith('=>', i): return text[:i].strip(), text[i:]
            if ch == ';': return text[:i].strip(), ''
            if ch == '=' and text[i + 1:i + 2] != '=' and text[i - 1:i] not in '!<>=': return text[:i].strip(), text[i:]
    return text.strip(), ''


def parse_member(decl, type_name, kind):
    d = re.sub(r'\s+', ' ', decl).strip()
    d = re.sub(r'^(\[[^\]]*\]\s*)+', '', d)
    is_protected = d.startswith('public protected ')
    if is_protected: d = 'public ' + d[len('public protected '):]
    head, body = split_body(d)
    head = re.sub(r'\)\s*:\s*(base|this)\s*\(.*$', ')', head)      # constructor chains
    depth, pstart = 0, -1
    for i, ch in enumerate(head):
        if ch == '<': depth += 1
        elif ch == '>': depth -= 1
        elif ch == '(' and depth == 0: pstart = i; break
    pre_paren = head[:pstart] if pstart >= 0 else head
    is_static = bool(re.search(r'\b(static|const)\b', pre_paren))
    m_kind, name, mtype = "field", None, ""
    readonly = False
    if re.search(r'\bevent\b', pre_paren):
        m_kind = "event"; name = pre_paren.split()[-1]
        mtype = pre_paren.split()[-2] if len(pre_paren.split()) > 1 else ""
    elif re.search(r'\boperator\b', pre_paren):
        m_kind = "operator"; name = "operator " + pre_paren.split("operator", 1)[1].strip()
    elif '(' in head and not re.search(r'\bthis\s*\[', pre_paren):
        words = pre_paren.split()
        name = re.sub(r'<.*$', '', words[-1]) if words else ""
        m_kind = "constructor" if name == type_name else "method"
        if m_kind == "method":
            pre = re.sub(r'^public\s+', '', pre_paren)
            pre = re.sub(r'\b' + KEYWORDS + r'\b\s*', '', pre).strip()
            mtype = pre.rsplit(' ', 1)[0] if ' ' in pre else ""
    else:
        words = re.sub(r'^public\s+', '', head)
        words = re.sub(r'\b' + KEYWORDS + r'\b\s*', '', words).strip()
        if 'this[' in words:
            m_kind = "indexer"; mtype, name = words.split(' this[', 1)[0], "this[" + words.split('this[', 1)[1]
        else:
            parts = words.rsplit(' ', 1)
            if len(parts) == 2: mtype, name = parts
            else: name = parts[0]
        if body.startswith('{') or body.startswith('=>'):
            if m_kind != "indexer": m_kind = "property"
            if body.startswith('=>'): readonly = True
            else:
                acc = body
                has_set = re.search(r'(?<!private )(?<!internal )(?<!protected )\b(set|init)\b', acc)
                readonly = not has_set
        if re.search(r'\bconst\b', pre_paren): m_kind = "constant"
        if m_kind == "field" and re.search(r'\breadonly\b', pre_paren): readonly = True
    params = []
    if m_kind in ("method", "constructor", "operator") and '(' in head:
        inner = head[pstart + 1: head.rindex(')')] if pstart >= 0 and ')' in head else ""
        depth, cur, items = 0, "", []
        for ch in inner:
            if ch in '<([': depth += 1
            if ch in '>)]': depth -= 1
            if ch == ',' and depth == 0: items.append(cur); cur = ""
            else: cur += ch
        if cur.strip(): items.append(cur)
        for it in items:
            it = it.strip()
            default = None
            if '=' in it:
                it, default = [x.strip() for x in it.split('=', 1)]
            ws = it.split()
            if len(ws) >= 2:
                params.append({"name": ws[-1], "type": " ".join(ws[:-1]), "default": default})
    pretty = re.sub(r'^public\s+', '', head)
    pretty = re.sub(r'\s*:\s*(base|this)\s*\(.*$', '', pretty) if m_kind == "constructor" else pretty
    if m_kind == "property": pretty += " { get; }" if readonly else " { get; set; }"
    if m_kind == "constant" and body.startswith('='): pretty += " " + body.rstrip(';').strip()
    return {"kind": m_kind, "name": name, "type": mtype.strip(), "static": is_static,
            "signature": ("protected " if is_protected else "") + pretty.strip(), "params": params, "readonly": readonly,
            "protected": is_protected}


def parse_file(path, source_tag):
    lines = open(path, encoding="utf-8-sig", errors="replace").read().split("\n")
    state = {"block": False}
    depth = 0
    namespace = ""
    ns_depth = -1
    stack = []          # open types: dict with depth at which its body sits
    types = []
    comments = []
    pending_type = None
    collecting = None   # member declaration spanning lines
    in_enum_body = None
    for raw in lines:
        line = raw.rstrip("\r")
        s = line.strip()
        code = strip_code(line, state)
        if s.startswith("//"):
            comments.append(s)
            continue
        if not s:
            if not collecting: comments = [] if len(comments) and not comments[-1].startswith("///") else comments
            continue
        cur_type = stack[-1] if stack else None
        body_level = cur_type and depth == cur_type["body_depth"]

        m_ns = re.match(r'^\s*namespace\s+([\w\.]+)', code)
        if m_ns:
            namespace = m_ns.group(1)
        mt = TYPE_RE.match(code)
        if mt and not collecting:
            mods = (mt.group(1) or "")
            full_mods = re.findall(r'\b(static|sealed|abstract|partial)\b', code.split(mt.group(2))[0])
            parent = cur_type["name"] if cur_type else None
            t = {"name": mt.group(3) + (mt.group(4) or "").replace(" ", ""), "simple": mt.group(3),
                 "kind": mt.group(2), "namespace": namespace, "modifiers": full_mods,
                 "base": (mt.group(5) or "").strip().rstrip(',').strip(),
                 "summary": clean_comment(comments), "members": [], "file": os.path.basename(path),
                 "source": source_tag, "parent": parent,
                 "enumValues": [] if mt.group(2) == "enum" else None}
            if t["base"]:
                t["base"] = re.sub(r'\s*where\s.*$', '', t["base"]).strip()
            if parent:
                t["name"] = parent + "." + t["name"]
            # private outer types make nested publics unreachable
            if cur_type is None or cur_type.get("public", True):
                t["public"] = True
            pending_type = t
            comments = []
            # a whole type on one line: "public enum Space { World, Self }"
            if '{' in code and code.count('{') == code.count('}'):
                body = code[code.index('{') + 1: code.rindex('}')]
                if t["kind"] == "enum":
                    for part in body.split(','):
                        nm = re.match(r'^\s*(\w+)\s*(?:=\s*(.+?))?\s*$', part)
                        if nm: t["enumValues"].append({"name": nm.group(1), "value": (nm.group(2) or "").strip(), "summary": ""})
                else:
                    for piece in re.split(r'(?<=[;}])\s*', body):
                        piece = piece.strip()
                        if MEMBER_START.match(piece):
                            e = parse_member(piece, t["simple"], t["kind"])
                            e["summary"] = ""
                            if e["name"]: t["members"].append(e)
                if t.get("public"): types.append(t)
                pending_type = None
                continue
        elif collecting is not None:
            collecting["text"] += " " + code.strip()
            collecting["bal"] += code.count('{') - code.count('}')
            ct = collecting["code"] = collecting["code"] + " " + code
            if collecting["bal"] <= 0 and (';' in code or '}' in code) and ct.count('(') <= ct.count(')'):
                collecting["done"] = True
        elif cur_type and body_level and cur_type["kind"] == "enum":
            for part in code.split(','):
                p = part.strip().rstrip('}').strip()
                if not p or p.startswith('{'): continue
                nm = re.match(r'^(\w+)\s*(?:=\s*(.+))?$', p)
                if nm:
                    cur_type["enumValues"].append({"name": nm.group(1), "value": (nm.group(2) or "").strip(),
                                                   "summary": clean_comment(comments) or clean_comment([line[line.find('//'):]] if '//' in line else [])})
            comments = []
        elif cur_type and body_level and cur_type.get("public") and (MEMBER_START.match(code) or PROTECTED_START.match(code)
                or (cur_type["kind"] == "interface" and re.match(r'^\s*[\w<>\[\],\s]+\s+\w+\s*(\(|\{)', code))):
            if PROTECTED_START.match(code): code = re.sub(r'^(\s*)protected\s+', r'public protected ', code)
            elif cur_type["kind"] == "interface" and not MEMBER_START.match(code): code = re.sub(r'^(\s*)', r'public ', code, count=1)
            trail = line[line.find('//'):] if '//' in line and '//' not in code else ""
            collecting = {"text": code.strip(), "code": code, "comments": comments[:] or ([trail] if trail else []),
                          "done": False, "type": cur_type, "bal": code.count('{') - code.count('}')}
            if collecting["bal"] <= 0 and (';' in code or '}' in code) and code.count('(') <= code.count(')'):
                collecting["done"] = True
            comments = []
        elif not s.startswith("["):
            comments = []

        if collecting is not None and collecting["done"]:
            text = collecting["text"]
            t = collecting["type"]
            trailing = ""
            # one public field line can declare several names
            mm = parse_member(text, t["simple"], t["kind"])
            doc = clean_comment(collecting["comments"]) or trailing.strip()
            names = [mm["name"]]
            if mm["kind"] == "field" and ',' in (text.split('=')[0]) and '(' not in text:
                head = re.sub(r'^(\[[^\]]*\]\s*)+', '', text.split(';')[0])
                head = re.sub(r'^public\s+', '', head)
                head = re.sub(r'\b' + KEYWORDS + r'\b\s*', '', head).strip()
                tparts = head.split(' ', 1)
                if len(tparts) == 2:
                    mm["type"] = tparts[0]
                    names = [re.sub(r'\s*=.*', '', x).strip() for x in tparts[1].split(',')]
            for nm in names:
                e = dict(mm); e["name"] = nm; e["summary"] = doc
                if len(names) > 1: e["signature"] = (("static " if e["static"] else "") + e["type"] + " " + nm)
                if e["name"] and not e["name"].startswith("operator") or e["kind"] == "operator":
                    t["members"].append(e)
            collecting = None

        # brace tracking
        for ch in code:
            if ch == '{':
                depth += 1
                if pending_type is not None:
                    pending_type["body_depth"] = depth
                    stack.append(pending_type)
                    if pending_type.get("public"): types.append(pending_type)
                    pending_type = None
            elif ch == '}':
                if stack and depth == stack[-1]["body_depth"]:
                    stack.pop()
                depth -= 1
        if pending_type is not None and code.rstrip().endswith(';'):
            # record struct / empty declaration
            if pending_type.get("public"): types.append(pending_type)
            pending_type = None
    for t in types:
        t.pop("body_depth", None)
    return types


def main():
    root = sys.argv[1] if len(sys.argv) > 1 else DEFAULT_ROOT
    out = []
    sharp = os.path.join(root, "ElintriaSharp")
    for f in sorted(os.listdir(sharp)):
        if f.endswith(".cs"):
            out += parse_file(os.path.join(sharp, f), "ElintriaSharp")
    pk = os.path.join(root, "Packages")
    for p in sorted(os.listdir(pk)):
        cs = os.path.join(pk, p, "csharp")
        if os.path.isdir(cs):
            for f in sorted(os.listdir(cs)):
                if f.endswith(".cs"):
                    out += parse_file(os.path.join(cs, f), "Package:" + p)
    # partial classes split across files -> one entry
    merged = {}
    for t in out:
        key = (t["namespace"], t["name"])
        if key in merged:
            m = merged[key]
            names = {(x["name"], x["signature"]) for x in m["members"]}
            m["members"] += [x for x in t["members"] if (x["name"], x["signature"]) not in names]
            if not m["summary"]: m["summary"] = t["summary"]
            if t["file"] not in m["file"]: m["file"] += ", " + t["file"]
        else:
            merged[key] = t
    res = list(merged.values())
    json.dump(res, open(os.path.join(HERE, "api_raw.json"), "w", encoding="utf-8"), indent=1)
    print(len(res), "types")
    for t in res:
        print("%-10s %-24s %-38s %3d  %s" % (t["kind"], t["namespace"], t["name"], len(t["members"]) + len(t["enumValues"] or []), t["source"] + "/" + t["file"]))


if __name__ == "__main__":
    main()
