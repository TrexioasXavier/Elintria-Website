namespace DocCheck_Json
{
using System.Collections.Generic;
using Elintria;

public class Settings : ElintriaScript
{
    public override void Start()
    {
        string text = "{ \"player\": { \"speed\": 5.5, \"name\": \"Ash\" }, \"music\": true, \"levels\": [\"Forest\", \"Cave\"] }";
        var root = (Dictionary<string, object>)Json.Parse(text);
        var player = Json.Obj(root, "player");

        float speed = Json.Float(player, "speed", 4f);
        string playerName = Json.Str(player, "name", "Player");
        bool music = Json.Bool(root, "music");
        string[] levels = Json.Strings(root, "levels");

        Debug.Log(playerName + " runs at " + speed + "; music " + music + "; " + levels.Length + " levels");
    }
}
}
