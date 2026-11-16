namespace DocCheck_FindObjectsSortMode
{
using Elintria;

public class RoundRobin : ElintriaScript
{
    int next;

    public override void Update()
    {
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.InstanceID);
        if (lights.Length == 0) return;
        next = (next + 1) % lights.Length;
        lights[next].intensity = 0.5f + Random.value;   // flicker one light a frame
    }
}
}
