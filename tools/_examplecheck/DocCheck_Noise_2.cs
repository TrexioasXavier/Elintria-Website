namespace DocCheck_Noise
{
using Elintria;
using Elintria.Procedural;

public class LavaGlow : ElintriaScript
{
    public override void Update()
    {
        Light l = GetComponent<Light>();
        if (l != null) l.intensity = 2f + Noise.Perlin(0.3f, 0.7f, Time.time * 0.8f) * 1.5f;
    }

    public static bool IsCellEdge(float x, float z)
    {
        float f1 = Noise.Worley(x * 0.5f, z * 0.5f, out float f2);
        return f2 - f1 < 0.08f;   // close to two cells at once: a crack between stones
    }
}
}
