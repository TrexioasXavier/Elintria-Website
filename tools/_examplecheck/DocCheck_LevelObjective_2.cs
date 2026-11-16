namespace DocCheck_LevelObjective
{
using Elintria;
using Elintria.Standard;

public class ObjectiveHud : ElintriaScript
{
    public LevelObjective objective;

    public override void Update()
    {
        objective.showHud = false;
        ScreenGUI.BeginFrame();
        ScreenGUI.Label(new Rect(20f, 20f, 400f, 30f), objective.Text(), null, Color.yellow);
    }
}
}
