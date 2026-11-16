namespace DocCheck_StdScreen
{
using Elintria;
using Elintria.Standard;

public class MousePick : ElintriaScript
{
    public override void Update()
    {
        float x, y;
        if (!Input.GetMouseButtonDown(0) || !GameWindow.MousePosition(out x, out y)) return;
        Vector3 o, d;
        StdScreen.ScreenRay(new Vector2(x, y), out o, out d);
        RaycastHit hit;
        if (Physics.Raycast(o, d, out hit, 100f)) Debug.Log("Clicked " + hit.gameObject.name);
    }
}
}
