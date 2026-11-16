namespace DocCheck_Content
{
using System.Collections.Generic;
using Elintria;

public class ShopData : ElintriaScript
{
    public string file = "Assets/Data/shop.json";

    public override void Start()
    {
        string json = Content.ReadAllText(file);
        if (json == null) { Debug.LogError(file + " not found"); return; }
        var root = Json.Parse(json) as Dictionary<string, object>;
        foreach (object o in Json.List(root, "items"))
        {
            var item = (Dictionary<string, object>)o;
            Debug.Log(Json.Str(item, "name") + " costs " + Json.Int(item, "price"));
        }
    }
}
}
