namespace DocCheck_Light
{
using Elintria;

public class DayNight : ElintriaScript
{
    public float dayLengthSeconds = 120f;
    Light sun;

    public override void Start()
    {
        sun = GetComponent<Light>();
        sun.type = LightType.Directional;
        sun.shadows = true;
    }

    public override void Update()
    {
        float t = (Time.time / dayLengthSeconds) % 1f;            // 0..1 through the day
        float angle = t * 360f - 90f;                               // sunrise at t = 0
        transform.rotation = Quaternion.Euler(angle, 30f, 0f);
        float up = Mathf.Clamp01(Mathf.Sin(t * Mathf.PI * 2f));     // 0 at night, 1 at noon
        sun.intensity = Mathf.Lerp(0.05f, 1.3f, up);
        sun.color = Color.Lerp(new Color(1f, 0.5f, 0.3f), Color.white, up);
    }
}
}
