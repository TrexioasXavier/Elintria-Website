namespace DocCheck_GpuTexture
{
using Elintria;

public class SecurityMonitor : ElintriaScript
{
    public GameObject cameraSpot;
    int screen;

    public override void Start() { screen = GpuTexture.CreateRender(320, 180); }

    public override void Update()
    {
        Transform c = cameraSpot.transform;
        RenderCamera.Render(screen, c.position, c.forward, Vector3.up, 60f, 0.1f, 80f);
        ScreenGUI.Image(new Rect(ScreenGUI.screenWidth - 340, 20, 320, 180), screen, Color.white);
    }

    public override void OnDestroy() { GpuTexture.Destroy(screen); }
}
}
