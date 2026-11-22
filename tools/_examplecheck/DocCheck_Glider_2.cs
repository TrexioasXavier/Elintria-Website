namespace DocCheck_Glider
{
using Elintria;
using Elintria.Standard;

public class Altimeter : ElintriaScript
{
    public override void Update()
    {
        Glider g = GetComponent<Glider>();
        if (g.gliding) ScreenGUI.Text(20f, 100f, Mathf.Round(g.HeightAboveGround()) + " m", null, Color.white);
    }
}
}
