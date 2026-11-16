namespace DocCheck_Quality
{
using Elintria;

public class CustomQuality : ElintriaScript
{
    public override void Start()
    {
        Quality.antiAliasing = 4;
        Quality.shadows = 2;               // hard and soft
        Quality.shadowDistance = 80f;
        Quality.renderScale = 0.9f;
        Quality.vSyncCount = 0;
        Quality.targetFrameRate = 144;
    }
}
}
