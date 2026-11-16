namespace DocCheck_Json
{
using System;
using Elintria;

public class ModLoader : ElintriaScript
{
    public string modFile = "Assets/Mods/mod.json";

    public override void Start()
    {
        string text = Content.ReadAllText(modFile) ?? "";
        object mod = Json.TryParse(text, null);
        if (mod == null)
        {
            try { Json.Parse(text); }
            catch (FormatException e) { Debug.LogWarning("Mod skipped: " + e.Message); }
            return;
        }
        Debug.Log("Mod loaded: " + Json.Stringify(mod));
    }
}
}
