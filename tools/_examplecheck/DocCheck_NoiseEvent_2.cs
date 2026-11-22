namespace DocCheck_NoiseEvent
{
using Elintria;
using Elintria.Standard;

public class NoiseAlarm : ElintriaScript
{
    public override void Update()
    {
        StdNoise.NoiseEvent n;
        if (!StdNoise.Heard(transform.position, 1.5f, 1f, out n)) return;
        string who = n.source != null ? n.source.name : "something";
        float age = Time.time - n.time;
        Debug.Log("Heard " + who + " " + age.ToString("0.0") + " s ago, " + n.radius + " m noise");
    }
}
}
