/* A small C# / JSON highlighter: keywords, types, strings, numbers, comments, methods, attributes.
   ElintriaHighlight.code(text, lang) -> HTML string. */
(function () {
  var KEYWORDS = ("abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using var virtual void volatile while yield async await get set value where nameof when record init").split(" ");
  var CONTROL = ("if else for foreach while do switch case break continue return yield throw try catch finally goto await using new").split(" ");
  var BUILTIN = ("bool byte char decimal double float int long object sbyte short string uint ulong ushort void var dynamic").split(" ");
  var kw = {}, ctl = {}, bi = {};
  KEYWORDS.forEach(function (k) { kw[k] = 1; });
  CONTROL.forEach(function (k) { ctl[k] = 1; });
  BUILTIN.forEach(function (k) { bi[k] = 1; });
  var KNOWN_TYPES = {};

  function esc(s) { return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;"); }
  function span(cls, s) { return '<span class="tok-' + cls + '">' + esc(s) + "</span>"; }

  function csharp(src) {
    var out = "", i = 0, n = src.length, prevWord = "";
    while (i < n) {
      var c = src[i], rest = src.slice(i), m;
      if (c === "/" && src[i + 1] === "/") {
        var e = src.indexOf("\n", i); if (e < 0) e = n;
        out += span("comment", src.slice(i, e)); i = e; continue;
      }
      if (c === "/" && src[i + 1] === "*") {
        var e2 = src.indexOf("*/", i + 2); e2 = e2 < 0 ? n : e2 + 2;
        out += span("comment", src.slice(i, e2)); i = e2; continue;
      }
      if (c === '"' || ((c === "@" || c === "$") && src[i + 1] === '"') || (c === "$" && src[i + 1] === "@")) {
        var j = i, verbatim = false;
        while (src[j] === "@" || src[j] === "$") { if (src[j] === "@") verbatim = true; j++; }
        j++;
        while (j < n) {
          if (!verbatim && src[j] === "\\") { j += 2; continue; }
          if (src[j] === '"') { if (verbatim && src[j + 1] === '"') { j += 2; continue; } j++; break; }
          if (src[j] === "\n" && !verbatim) break;
          j++;
        }
        out += span("string", src.slice(i, j)); i = j; continue;
      }
      if (c === "'") {
        m = /^'(\\.|[^'\\])'/.exec(rest);
        if (m) { out += span("string", m[0]); i += m[0].length; continue; }
      }
      if (c === "[" && (m = /^\[(\s*[A-Z]\w*)(\s*\(|\s*\])/.exec(rest)) && /(^|\n)\s*$/.test(src.slice(Math.max(0, i - 40), i))) {
        out += "[" + span("attr", m[1]); i += 1 + m[1].length; continue;
      }
      if ((m = /^(0x[0-9a-fA-F]+|\d+(\.\d+)?([eE][+-]?\d+)?[fFdDmMuUlL]*)/.exec(rest)) && !/\w/.test(src[i - 1] || "")) {
        out += span("number", m[0]); i += m[0].length; continue;
      }
      if ((m = /^[A-Za-z_]\w*/.exec(rest))) {
        var w = m[0], after = src.slice(i + w.length);
        var cls = null;
        if (ctl[w]) cls = "control";
        else if (bi[w]) cls = "builtin";
        else if (kw[w]) cls = "keyword";
        else if (/^\s*(<[^<>()]*>)?\s*\(/.test(after) && prevWord !== "new") cls = "method";
        else if (/^[A-Z]/.test(w) && (KNOWN_TYPES[w] || prevWord === "new" || prevWord === "class" || prevWord === "struct" || prevWord === "enum" || prevWord === "interface" || /^\s*(<[^()]*>)?\s+[a-z_]\w*\s*[=;,)(]/.test(after) || /^\s*\./.test(after) && KNOWN_TYPES[w] || /^\s*\[\]/.test(after) || /^\s*>/.test(after))) cls = "type";
        out += cls ? span(cls, w) : esc(w);
        prevWord = w; i += w.length; continue;
      }
      if (!/\s/.test(c)) prevWord = c === "." ? prevWord : "";
      if (/[{}()[\];,.<>=+\-*/%!&|^~?:]/.test(c)) { out += '<span class="tok-punct">' + esc(c) + "</span>"; i++; continue; }
      out += esc(c); i++;
    }
    return out;
  }

  function json(src) {
    return esc(src)
      .replace(/(&quot;|")((?:\\.|[^"\\])*)"(\s*:)?/g, function (all, q, body, colon) {
        return colon ? '<span class="tok-property">"' + body + '"</span>' + colon : '<span class="tok-string">"' + body + '"</span>';
      })
      .replace(/\b(true|false|null)\b/g, '<span class="tok-keyword">$1</span>')
      .replace(/(^|[^\w"#])(-?\d+(\.\d+)?)/g, '$1<span class="tok-number">$2</span>');
  }

  window.ElintriaHighlight = {
    setTypes: function (names) { names.forEach(function (t) { KNOWN_TYPES[t] = 1; }); },
    code: function (text, lang) {
      if (lang === "json") return json(text);
      if (lang === "text" || lang === "plain") return esc(text);
      return csharp(text);
    }
  };
})();
