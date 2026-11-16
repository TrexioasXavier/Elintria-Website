namespace DocCheck_Light
{
using Elintria;

public class Torch : ElintriaScript
{
    public float baseIntensity = 1.6f;
    public float flicker = 0.4f;
    Light lamp;

    public override void Start() { lamp = GetComponent<Light>(); }

    public override void Update()
    {
        float n = Mathf.PerlinNoise(Time.time * 6f, 0.7f);
        lamp.intensity = baseIntensity + (n - 0.5f) * 2f * flicker;
        lamp.color = Color.Lerp(new Color(1f, 0.55f, 0.2f), new Color(1f, 0.75f, 0.35f), n);
    }
}
}
