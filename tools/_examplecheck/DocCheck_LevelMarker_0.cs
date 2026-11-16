namespace DocCheck_LevelMarker
{
using Elintria;

public class LevelLights : ElintriaScript
{
    public GameObject lampTemplate;

    public override void Start()
    {
        GeneratedLevel level = LevelGenerator.Generate("dark facility", "seed=8");
        if (level == null) return;
        foreach (LevelMarker m in level.Markers("Light"))
        {
            if (m.intensity <= 0f) continue;          // a dead fitting
            GameObject lamp = Instantiate(lampTemplate, m.position, Quaternion.identity);
            Light l = lamp.GetComponent<Light>();
            if (l != null) { l.color = m.color; l.intensity = m.intensity; l.range = m.range; }
        }
    }
}
}
