namespace DocCheck_Color32
{
using Elintria;

public class HexColor : ElintriaScript
{
    public static string ToHex(Color c)
    {
        Color32 b = c;   // implicit conversion
        return "#" + b.r.ToString("X2") + b.g.ToString("X2") + b.b.ToString("X2");
    }

    public override void Start()
    {
        Debug.Log(ToHex(new Color(1f, 0.5f, 0f)));   // #FF7F00
    }
}
}
