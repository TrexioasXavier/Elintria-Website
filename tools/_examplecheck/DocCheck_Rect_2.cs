namespace DocCheck_Rect
{
using Elintria;

public class DragBox : ElintriaScript
{
    Rect box = new Rect(100, 100, 80, 80);
    Rect bin = new Rect(400, 300, 120, 120);
    bool dragging;

    public override void Update()
    {
        Vector2 m = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && box.Contains(m)) dragging = true;
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (dragging) box.position = m - box.size * 0.5f;

        ScreenGUI.Fill(bin, box.Overlaps(bin) ? Color.red : Color.gray);
        ScreenGUI.Fill(box, Color.cyan);
    }
}
}
