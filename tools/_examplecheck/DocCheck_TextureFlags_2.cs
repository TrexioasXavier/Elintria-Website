namespace DocCheck_TextureFlags
{
using Elintria;

public class UiTex : ElintriaScript
{
    public override void Start()
    {
        GpuTexture.Load(Project.Resolve("Assets/UI/panel.png"), TextureFlags.Linear | TextureFlags.Clamp, out _, out _);
    }
}
}
