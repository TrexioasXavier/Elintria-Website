namespace DocCheck_ActionBuffers
{
using Elintria;
using Elintria.ML;

public class KartAgent : Agent
{
    public override int ContinuousActions => 2;

    public override void OnActionReceived(ActionBuffers a)
    {
        float steer = a.continuous[0], throttle = a.continuous[1];
        transform.Rotate(0f, steer * 3f, 0f);
        transform.position += transform.forward * throttle * 0.2f;
    }

    public override void CollectObservations(Observations o) { o.Add(transform.forward); }
}
}
