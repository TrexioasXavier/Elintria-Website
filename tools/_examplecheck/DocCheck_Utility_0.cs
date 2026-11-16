namespace DocCheck_Utility
{
using Elintria;
using Elintria.AI;

public class CurvePlot : ElintriaScript
{
    public override void Update()
    {
        string[] kinds = { "linear", "inverse", "quadratic", "sqrt", "logistic", "step", "bell" };
        for (int k = 0; k < kinds.Length; k++)
            for (int i = 0; i < 50; i++)
            {
                float y0 = Utility.Curve(kinds[k], i / 50f), y1 = Utility.Curve(kinds[k], (i + 1) / 50f);
                ScreenGUI.Line(new Vector2(40 + i * 4, 120 + k * 70 - y0 * 50), new Vector2(44 + i * 4, 120 + k * 70 - y1 * 50), Color.cyan);
            }
    }
}
}
