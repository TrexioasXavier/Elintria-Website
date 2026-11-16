namespace DocCheck_Weather
{
using Elintria;
using Elintria.Standard;

public class FinaleStorm : ElintriaScript
{
    public void Begin()
    {
        Weather w = FindObjectOfType<Weather>();
        if (w == null) return;
        w.changeMinutes = 0f;
        w.Set(3);
    }
}
}
