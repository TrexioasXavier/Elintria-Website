namespace DocCheck_StdScreen
{
using Elintria;
using Elintria.Standard;

public class FloatingLabel : ElintriaScript
{
    public string text = "Here";

    public override void Update()
    {
        Vector2 s;
        if (StdScreen.WorldToScreen(transform.position + Vector3.up * 2f, out s)) ScreenGUI.Text(s.x, s.y, text);
    }
}
}
