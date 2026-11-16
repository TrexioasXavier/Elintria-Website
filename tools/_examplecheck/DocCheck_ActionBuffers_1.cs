namespace DocCheck_ActionBuffers
{
using Elintria;
using Elintria.ML;

public class TankAgent : Agent
{
    public override int ContinuousActions => 0;
    public override int[] DiscreteBranches => new[] { 3, 2 };   // turn: left/none/right, fire: no/yes

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.Rotate(0f, (a.discrete[0] - 1) * 4f, 0f);
        if (a.discrete[1] == 1) Debug.Log("Bang");
    }

    public override void CollectObservations(Observations o) { o.Add(transform.forward); }

    public override void Heuristic(ActionBuffers a)
    {
        a.discrete[0] = Input.GetKey(KeyCode.A) ? 0 : Input.GetKey(KeyCode.D) ? 2 : 1;
        a.discrete[1] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
}
