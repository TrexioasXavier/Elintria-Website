namespace DocCheck_StdScreen
{
using Elintria;
using Elintria.Standard;

public class OnScreenCheck : ElintriaScript
{
    public bool onScreen;

    public override void Update()
    {
        Vector2 s;
        onScreen = StdScreen.WorldToScreen(transform.position, out s) && s.x >= 0f && s.y >= 0f && s.x <= GameWindow.width && s.y <= GameWindow.height;
    }
}
}
