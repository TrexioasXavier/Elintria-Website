namespace DocCheck_Weather
{
using Elintria;
using Elintria.Standard;

public class RainPutsOutFires : ElintriaScript
{
    public override void Update()
    {
        Weather w = FindObjectOfType<Weather>();
        bool raining = w != null && (w.weather == 1 || w.weather == 3) && w.strength > 0.5f;
        GetComponent<HeatSource>().enabled = !raining;
    }
}
}
