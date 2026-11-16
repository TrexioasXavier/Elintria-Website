namespace DocCheck_TextureFlags
{
using Elintria;

public class GroundTex : ElintriaScript
{
    public override void Start()
    {
        GpuTexture.Load(Project.Resolve("Assets/Textures/grass.png"), TextureFlags.Linear | TextureFlags.Mipmaps, out _, out _);
    }
}
}
