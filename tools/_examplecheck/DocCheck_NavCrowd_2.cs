namespace DocCheck_NavCrowd
{
using Elintria;

public class CrowdStats : ElintriaScript
{
    public override void Update()
    {
        float[] s = NavCrowd.Stats();
        ScreenGUI.Text(20, 20, "Crowd: " + s[1] + " agents, " + s[0].ToString("0.00") + " ms, " + s[2] + " skipped");
    }
}
}
