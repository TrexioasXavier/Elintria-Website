namespace DocCheck_Json
{
using System;
using System.Collections.Generic;
using System.IO;
using Elintria;

public class SaveGame : ElintriaScript
{
    string SavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyGame", "save.json");

    public void Save(int level, float health, List<object> items)
    {
        var data = new Dictionary<string, object> { { "level", level }, { "health", health }, { "items", items } };
        Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
        File.WriteAllText(SavePath, Json.Stringify(data, true));
    }

    public int LoadLevel()
    {
        if (!File.Exists(SavePath)) return 1;
        var data = Json.TryParse(File.ReadAllText(SavePath)) as Dictionary<string, object>;
        return data == null ? 1 : Json.Int(data, "level", 1);
    }
}
}
