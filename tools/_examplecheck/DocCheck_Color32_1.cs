namespace DocCheck_Color32
{
using Elintria;

public class Checker : ElintriaScript
{
    public static byte[] Make(int size, Color32 a, Color32 b)
    {
        byte[] rgba = new byte[size * size * 4];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                Color32 c = ((x / 8 + y / 8) % 2 == 0) ? a : b;
                int i = (y * size + x) * 4;
                rgba[i] = c.r; rgba[i + 1] = c.g; rgba[i + 2] = c.b; rgba[i + 3] = c.a;
            }
        return rgba;
    }

    public override void Start()
    {
        int tex = GpuTexture.Create(64, 64, Make(64, new Color32(255, 255, 255, 255), new Color32(40, 40, 40, 255)), TextureFlags.Nearest);
        Debug.Log("Texture id " + tex);
    }
}
}
