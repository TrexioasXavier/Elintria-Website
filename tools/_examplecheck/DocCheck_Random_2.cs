namespace DocCheck_Random
{
using System;
using Elintria;

public class DailyChallenge : ElintriaScript
{
    public override void Start()
    {
        int seed = DateTime.UtcNow.Year * 1000 + DateTime.UtcNow.DayOfYear;
        Elintria.Random.InitState(seed);   // "Elintria." because System also has a Random
        for (int i = 0; i < 5; i++)
            Debug.Log("Enemy " + i + " at " + (Elintria.Random.onUnitSphere * 20f));
    }
}
}
