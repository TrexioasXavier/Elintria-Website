namespace DocCheck_Overlay2D
{
using Elintria;

public class SheetFrame : ElintriaScript
{
    public int sheet;   // texture id set elsewhere

    public override void Update()
    {
        int frame = (int)(Time.time * 8f) % 16;
        float u = (frame % 4) / 4f, v = (frame / 4) / 4f;
        float[] q = { sheet, 100, 100, 64, 64, u, v, u + 0.25f, v + 0.25f, 1, 1, 1, 1, 0 };
        Overlay2D.Submit(q, 1);
    }
}
}
