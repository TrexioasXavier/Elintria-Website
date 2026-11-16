namespace DocCheck_NetWriter
{
using Elintria.Netcode;

public static class ScoreMessage
{
    public static string Make(int team, int score, float time) => new NetWriter().Put("score").Put(team).Put(score).Put(time).ToString();
}
}
