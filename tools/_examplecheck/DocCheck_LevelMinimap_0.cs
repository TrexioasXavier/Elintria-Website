namespace DocCheck_LevelMinimap
{
using Elintria;
using Elintria.Standard;

public class MapSettings : ElintriaScript
{
    public override void Start()
    {
        LevelMinimap map = FindObjectOfType<LevelMinimap>();
        if (map == null) return;
        map.size = 300f;
        map.range = 60f;
        map.corner = 0;
    }
}
}
