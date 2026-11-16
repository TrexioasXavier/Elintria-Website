namespace DocCheck_Mathf
{
using Elintria;

public class Lantern : ElintriaScript
{
    public GameObject lamp;   // an object with a Light

    public override void Update()
    {
        float swing = Mathf.Sin(Time.time * 1.5f) * 15f;                  // -15..15 degrees
        transform.localEulerAngles = new Vector3(0f, 0f, swing);

        float glide = Mathf.PingPong(Time.time * 0.5f, 2f) - 1f;          // -1..1 linear
        transform.localPosition = new Vector3(glide * 0.2f, transform.localPosition.y, 0f);

        Light l = lamp != null ? lamp.GetComponent<Light>() : null;
        if (l != null) l.intensity = Mathf.Lerp(1.5f, 2.5f, Mathf.PerlinNoise(Time.time * 4f, 0.3f));
    }
}
}
