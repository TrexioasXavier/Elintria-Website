namespace DocCheck_HideInInspectorAttribute
{
using Elintria;

public class Score : ElintriaScript
{
    public int pointsPerCoin = 10;         // a setting: shown
    [HideInInspector] public int score;    // state: hidden

    public void AddCoin() { score += pointsPerCoin; }
}
}
