"""Compiles every C# example in tools/content/*.md against the engine's scripting library.

    python check_examples.py [engine_root]

Needs the .NET 8 SDK and a built ElintriaSharp (ElintriaSharp/obj/Release/ElintriaSharp.dll).
Each documented type's examples go into one namespace (so examples on the same page may
use each other's classes); each example keeps its own using directives. Errors are
reported against the type and example title they came from.
"""
import os, re, sys, subprocess, shutil
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import build_docs

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = sys.argv[1] if len(sys.argv) > 1 else os.path.normpath(os.path.join(HERE, "..", "..", "Engine", "ElintriaGameEngine"))
OUT = os.path.join(HERE, "_examplecheck")


def main():
    content = build_docs.parse_content()
    if os.path.isdir(OUT): shutil.rmtree(OUT)
    os.makedirs(OUT)
    origin = {}   # file name -> (type, example title)
    n = 0
    for key, entry in content.items():
        if entry["page"]: continue
        ns = "DocCheck_" + re.sub(r'\W', '_', key)
        for i, ex in enumerate(entry["examples"]):
            if ex["lang"] not in ("csharp", "cs"): continue
            code = ex["code"]
            usings = [l for l in code.split("\n") if re.match(r'^\s*using\s+[\w\.]+\s*;\s*$', l)]
            body = "\n".join(l for l in code.split("\n") if l not in usings)
            fname = "%s_%d.cs" % (ns, i)
            with open(os.path.join(OUT, fname), "w", encoding="utf-8") as f:
                f.write("namespace %s\n{\n%s\n%s\n}\n" % (ns, "\n".join(usings), body))
            origin[fname] = (key, ex["title"])
            n += 1
    sharp = os.path.join(ROOT, "ElintriaSharp", "obj", "Release", "ElintriaSharp.dll")
    std = os.path.join(ROOT, "Packages", "Standard", "csharp", "*.cs")
    with open(os.path.join(OUT, "DocCheck.csproj"), "w", encoding="utf-8") as f:
        f.write('''<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <OutputType>Library</OutputType>
    <Nullable>disable</Nullable>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <NoWarn>CS0649;CS0414;CS0169;CS0168;CS0219;CS0162;CS0108;CS0114</NoWarn>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="*.cs" />
    <Compile Include="%s" />
    <Reference Include="ElintriaSharp"><HintPath>%s</HintPath></Reference>
  </ItemGroup>
</Project>
''' % (std, sharp))
    print("compiling %d examples..." % n)
    r = subprocess.run(["dotnet", "build", "-nologo", "-clp:NoSummary", "-v:q"], cwd=OUT, capture_output=True, text=True)
    errors = {}
    for line in (r.stdout + r.stderr).splitlines():
        m = re.search(r'([\w\.]+\.cs)\((\d+),\d+\): error (\w+): (.*?)(\s\[.*)?$', line)
        if not m: continue
        fname = os.path.basename(m.group(1))
        key = origin.get(fname, (fname, ""))
        errors.setdefault((key, m.group(3) + ": " + m.group(4)), int(m.group(2)))
    for ((key, msg), ln) in sorted(errors.items(), key=lambda x: x[0][0]):
        print("%s / %s (line %d): %s" % (key[0], key[1], ln, msg))
    print("errors:", len(errors))
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
