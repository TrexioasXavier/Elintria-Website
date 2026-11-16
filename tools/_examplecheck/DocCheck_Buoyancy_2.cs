namespace DocCheck_Buoyancy
{
using Elintria;
using Elintria.Standard;

public class LandingSplash : ElintriaScript
{
    bool was;

    public override void Update()
    {
        bool now = GetComponent<Buoyancy>().inWater;
        if (now && !was) Audio.PlayClipAtPoint("Audio/splash.wav", transform.position);
        was = now;
    }
}
}
