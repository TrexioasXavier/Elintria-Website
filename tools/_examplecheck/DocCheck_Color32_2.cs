namespace DocCheck_Color32
{
using Elintria;

public class PaletteMatch : ElintriaScript
{
    public static bool Same(Color32 x, Color32 y) => x.r == y.r && x.g == y.g && x.b == y.b && x.a == y.a;

    public override void Start()
    {
        Color32 wall = new Color32(0, 0, 0, 255);
        Debug.Log(Same(wall, Color.black) ? "black is wall" : "no match");
    }
}
}
