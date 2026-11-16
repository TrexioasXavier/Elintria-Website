namespace DocCheck_TextAnchor
{
using Elintria;

public class ScoreTable : ElintriaScript
{
    string[] names = { "Ash", "Brook", "Cyd" };
    int[] scores = { 1520, 980, 75 };

    public override void Update()
    {
        for (int i = 0; i < names.Length; i++)
        {
            Rect row = new Rect(100, 100 + i * 28, 240, 26);
            ScreenGUI.Label(row, names[i], null, Color.white, TextAnchor.MiddleLeft);
            ScreenGUI.Label(row, scores[i].ToString(), null, Color.white, TextAnchor.MiddleRight);
        }
    }
}
}
