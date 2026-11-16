namespace DocCheck_Observations
{
using Elintria;
using Elintria.ML;

public class StateAgent : Agent
{
    public int weapon;          // 0 sword, 1 bow, 2 staff
    public bool hasKey;
    public float health = 1f;

    public override int[] DiscreteBranches => new[] { 4 };
    public override int ContinuousActions => 0;

    public override void CollectObservations(Observations o)
    {
        o.AddOneHot(weapon, 3);
        o.Add(hasKey);
        o.Add(health);
        Debug.Assert(o.Count == 5);
    }

    public override void OnActionReceived(ActionBuffers a) { weapon = a.discrete[0] % 3; }
}
}
