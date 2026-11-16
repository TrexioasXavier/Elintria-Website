namespace DocCheck_GpuTexture
{
using Elintria;

public class PaintCanvas : ElintriaScript
{
    const int Size = 128;
    byte[] pixels = new byte[Size * Size * 4];
    int tex;

    public override void Start() { tex = GpuTexture.Create(Size, Size, pixels, TextureFlags.Nearest); }

    public override void Update()
    {
        Rect area = new Rect(20, 20, Size * 3, Size * 3);
        Vector2 m = Input.mousePosition;
        if (Input.GetMouseButton(0) && area.Contains(m))
        {
            int x = (int)((m.x - area.x) / 3), y = (int)((m.y - area.y) / 3);
            int i = (y * Size + x) * 4;
            pixels[i] = 255; pixels[i + 1] = 200; pixels[i + 2] = 40; pixels[i + 3] = 255;
            GpuTexture.Update(tex, pixels);
        }
        ScreenGUI.Image(area, tex, Color.white);
    }
}
}
