namespace DocCheck_LightType
{
using Elintria;

public class MakeLamp : ElintriaScript
{
    public override void Start()
    {
        GameObject g = new GameObject("Lamp");
        g.transform.position = transform.position + Vector3.up * 3f;
        Light l = g.AddComponent<Light>();
        l.type = LightType.Point;
        l.range = 8f;
        l.intensity = 2f;
        l.color = new Color(1f, 0.9f, 0.7f);
    }
}
}
