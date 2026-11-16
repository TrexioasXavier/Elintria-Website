namespace DocCheck_Debug
{
using System;
using Elintria;

public class WaveConfig : ElintriaScript
{
    public int waves = 5;
    public string dataFile = "Assets/Data/waves.json";

    public override void Start()
    {
        Debug.Assert(waves > 0, "waves must be at least 1");
        try
        {
            object data = Json.Parse(Content.ReadAllText(dataFile) ?? "");
            Debug.Log("Loaded " + dataFile);
        }
        catch (Exception e)
        {
            Debug.LogException(e, gameObject);
        }
    }
}
}
