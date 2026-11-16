namespace DocCheck_Noise
{
using Elintria;
using Elintria.Procedural;

public class CloudTexture : ElintriaScript
{
    public override void Start()
    {
        const int S = 128;
        byte[] px = new byte[S * S * 4];
        for (int y = 0; y < S; y++)
            for (int x = 0; x < S; x++)
            {
                float v = Noise.PerlinTileable(x / 16f, y / 16f, S / 16, S / 16) * 0.5f + 0.5f;
                byte b = (byte)(Mathf.Clamp01(v) * 255f);
                int i = (y * S + x) * 4;
                px[i] = b; px[i + 1] = b; px[i + 2] = b; px[i + 3] = 255;
            }
        int tex = GpuTexture.Create(S, S, px, TextureFlags.Linear | TextureFlags.Mipmaps);
        Debug.Log("Cloud texture " + tex);
    }
}
}
