namespace DocCheck_Vector4
{
using Elintria;

public class ColorMath : ElintriaScript
{
    public Vector4 tint = new Vector4(1f, 0.5f, 0.2f, 1f);

    public override void Start()
    {
        Color c = tint;                 // Vector4 -> Color
        Vector4 back = c * 0.5f;        // Color -> Vector4
        Debug.Log("Half bright: " + back);
    }
}
}
