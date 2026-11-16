namespace DocCheck_SaveSystem
{
using Elintria;
using Elintria.Standard;

public class SaveScore : ElintriaScript
{
    public static int score;

    public override void Start()
    {
        SaveSystem.Saving += d => d["score"] = score.ToString();
        SaveSystem.Loaded += d => { string s; if (d.TryGetValue("score", out s)) score = int.Parse(s); };
    }
}
}
