namespace DocCheck_Overlay2D
{
using Elintria;

public class SpinCrosshair : ElintriaScript
{
    int tex;
    readonly float[] q = new float[14];

    public override void Start() { tex = GpuTexture.Load(Project.Resolve("Assets/UI/crosshair.png"), TextureFlags.Linear, out _, out _); }

    public override void Update()
    {
        float s = 32f;
        float[] v = { tex, GameWindow.width / 2f - s / 2f, GameWindow.height / 2f - s / 2f, s, s, 0, 0, 1, 1, 1, 1, 1, 1, Time.time * 90f };
        v.CopyTo(q, 0);
        Overlay2D.Submit(q, 1);
    }
}
}
