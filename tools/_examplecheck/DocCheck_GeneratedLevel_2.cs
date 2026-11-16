namespace DocCheck_GeneratedLevel
{
using Elintria;

public class MapToggle : ElintriaScript
{
    public GeneratedLevel level;
    bool shown = true;

    public override void Update()
    {
        if (level == null || !Input.GetKeyDown(KeyCode.M)) return;
        shown = !shown;
        level.SetVisible(shown);
    }
}
}
