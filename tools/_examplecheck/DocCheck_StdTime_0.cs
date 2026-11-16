namespace DocCheck_StdTime
{
using Elintria;
using Elintria.Standard;

public class PauseClock : ElintriaScript
{
    public override void Update()
    {
        if (Time.timeScale == 0f) ScreenGUI.Text(20f, 20f, "Paused for " + Mathf.Round(StdTime.real) + " s");
    }
}
}
