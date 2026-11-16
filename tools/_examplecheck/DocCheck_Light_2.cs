namespace DocCheck_Light
{
using Elintria;

public class AlarmLight : ElintriaScript
{
    public bool alarm;
    Light spot;

    public override void Start()
    {
        spot = GetComponent<Light>();
        spot.type = LightType.Spot;
        spot.spotAngle = 40f;
        spot.range = 15f;
    }

    public override void Update()
    {
        spot.intensity = alarm ? 3f : 0f;
        if (!alarm) return;
        transform.Rotate(0f, 360f * Time.deltaTime, 0f, Space.World);
        spot.color = Color.red;
    }
}
}
