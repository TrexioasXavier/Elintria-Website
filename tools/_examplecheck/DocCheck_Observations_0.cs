namespace DocCheck_Observations
{
using Elintria;
using Elintria.ML;

public class ArenaAgent : Agent
{
    public GameObject enemy;
    public float arenaHalfSize = 10f;

    public override int ContinuousActions => 2;

    public override void CollectObservations(Observations o)
    {
        o.Add(transform.position / arenaHalfSize);                         // roughly -1..1
        o.Add((enemy.transform.position - transform.position) / arenaHalfSize);
        o.Add(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.position += new Vector3(a.continuous[0], 0f, a.continuous[1]) * 0.1f;
    }
}
}
