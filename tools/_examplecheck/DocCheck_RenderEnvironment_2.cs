namespace DocCheck_RenderEnvironment
{
using Elintria;

public class VideoOptions : ElintriaScript
{
    float gamma = 1f;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp)) gamma = Mathf.Min(2f, gamma + 0.1f);
        if (Input.GetKeyDown(KeyCode.PageDown)) gamma = Mathf.Max(0.5f, gamma - 0.1f);
        RenderEnvironment.SetBrightness(gamma);
        ScreenGUI.Text(20, 20, "Brightness " + gamma.ToString("0.0") + "   triangles " + RenderEnvironment.Stat(1));
    }
}
}
