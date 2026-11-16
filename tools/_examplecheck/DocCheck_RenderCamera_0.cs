namespace DocCheck_RenderCamera
{
using Elintria;

public class Minimap : ElintriaScript
{
    public GameObject player;
    int tex;

    public override void Start() { tex = GpuTexture.CreateRender(256, 256); }

    public override void Update()
    {
        Vector3 p = player.transform.position;
        RenderCamera.Render(tex, p + Vector3.up * 40f, Vector3.down, Vector3.forward, 50f, 1f, 100f);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth - 220, 20, 200, 200), tex, Color.white);
    }
}
}
