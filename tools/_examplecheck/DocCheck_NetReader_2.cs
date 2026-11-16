namespace DocCheck_NetReader
{
using Elintria;
using Elintria.Netcode;

public static class ScoreDecode
{
    public static void Apply(string message)
    {
        var r = new NetReader(message.Split('|'), 0);
        if (r.Str() != "score") return;
        int team = r.Int(), score = r.Int();
        float time = r.Float();
        Debug.Log("Team " + team + " has " + score + " at " + time + " s");
    }
}
}
