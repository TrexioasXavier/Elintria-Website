namespace DocCheck_Weather
{
using Elintria;
using Elintria.Standard;

public class MountainSnow : ElintriaScript
{
    public Weather weather;

    public override void Update()
    {
        if (weather != null) weather.Set(transform.position.y > 40f ? 2 : 0);
    }
}
}
