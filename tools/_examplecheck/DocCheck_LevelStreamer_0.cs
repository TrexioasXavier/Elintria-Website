namespace DocCheck_LevelStreamer
{
using Elintria;
using Elintria.Standard;

public class StreamQuality : ElintriaScript
{
    public override void Start()
    {
        LevelStreamer s = GetComponent<LevelStreamer>();
        s.streamDistance = Quality.level == 0 ? 20f : 45f;
    }
}
}
