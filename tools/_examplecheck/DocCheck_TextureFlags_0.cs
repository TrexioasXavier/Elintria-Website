namespace DocCheck_TextureFlags
{
using Elintria;

public class PixelArt : ElintriaScript
{
    public override void Start()
    {
        int tex = GpuTexture.Load(Project.Resolve("Assets/Sprites/hero.png"), TextureFlags.Nearest | TextureFlags.Clamp, out int w, out int h);
        Debug.Log("Loaded " + w + "x" + h);
    }
}
}
