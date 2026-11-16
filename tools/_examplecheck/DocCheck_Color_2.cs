namespace DocCheck_Color
{
using Elintria;

public class TeamColors : ElintriaScript
{
    public static Color ForTeam(int team)
    {
        Color[] palette = { new Color(0.9f, 0.2f, 0.2f), new Color(0.2f, 0.4f, 0.95f), new Color(0.2f, 0.8f, 0.3f) };
        return palette[Mathf.Abs(team) % palette.Length];
    }

    public static Color Darker(Color c, float amount)
    {
        return c * (1f - amount) + Color.black * amount;   // keeps alpha close to the original
    }

    public override void Start()
    {
        Debug.Log("Team 1 is " + ForTeam(1) + ", shaded " + Darker(ForTeam(1), 0.3f));
    }
}
}
