namespace DocCheck_RenderCamera
{
using Elintria;

public class RearMirror : ElintriaScript
{
    int tex;

    public override void Start() { tex = GpuTexture.CreateRender(300, 90); }

    public override void Update()
    {
        Vector3 eye = transform.position + Vector3.up * 1.2f;
        RenderCamera.Render(tex, eye, -transform.forward, Vector3.up, 50f, 0.3f, 120f, RenderCamera.ShowShadowsOnly);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth / 2 - 150, 10, 300, 90), tex, Color.white);
    }
}
}
