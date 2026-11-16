namespace DocCheck_Mouse
{
using Elintria;

public class CursorForMenus : ElintriaScript
{
    public bool menuOpen;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuOpen = !menuOpen;
            if (menuOpen) Mouse.ShowCursor(); else Mouse.HideCursor();
        }
    }
}
}
