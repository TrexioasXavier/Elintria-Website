namespace DocCheck_GpuTexture
{
using Elintria;

public class Logo : ElintriaScript
{
    int tex, w, h;

    public override void Start() { tex = GpuTexture.Load(Project.Resolve("Assets/UI/logo.png"), TextureFlags.Linear, out w, out h); }

    public override void Update()
    {
        if (tex != 0) ScreenGUI.Image(new Rect(20, 20, w * 0.5f, h * 0.5f), tex, Color.white);
    }

    public override void OnDestroy() { if (tex != 0) GpuTexture.Destroy(tex); }
}
}
