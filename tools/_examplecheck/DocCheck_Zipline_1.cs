namespace DocCheck_Zipline
{
using Elintria;
using Elintria.Standard;

public class ZipSpeed : ElintriaScript
{
    public override void Update()
    {
        foreach (Zipline z in FindObjectsOfType<Zipline>())
            if (z.riding) ScreenGUI.Text(20f, 80f, Mathf.Round(z.currentSpeed * 3.6f) + " km/h", null, Color.white);
    }
}
}
