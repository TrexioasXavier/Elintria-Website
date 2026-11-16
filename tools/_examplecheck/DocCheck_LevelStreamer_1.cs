namespace DocCheck_LevelStreamer
{
using Elintria;
using Elintria.Standard;

public class TwoZones : ElintriaScript
{
    public override void Start()
    {
        LevelStreamer s = AddComponent<LevelStreamer>();
        s.zones = "-20,0,-20,0,4,20;0,0,-20,20,4,20";
        s.neighbours = "0:1;1:0";
        s.ApplyZones();
    }
}
}
