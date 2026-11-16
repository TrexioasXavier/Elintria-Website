namespace DocCheck_Observations
{
using Elintria;
using Elintria.ML;

public class RayEyes : Agent
{
    public int rays = 7;
    public float range = 10f;

    public override int ContinuousActions => 2;

    public override void CollectObservations(Observations o)
    {
        for (int i = 0; i < rays; i++)
        {
            Vector3 dir = Quaternion.Euler(0f, -60f + 120f * i / (rays - 1), 0f) * transform.forward;
            bool hit = Physics.Raycast(transform.position, dir, out RaycastHit h, range);
            o.Add(hit ? h.distance / range : 1f);
        }
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        transform.Rotate(0f, a.continuous[0] * 5f, 0f);
        transform.position += transform.forward * Mathf.Max(0f, a.continuous[1]) * 0.1f;
    }
}
}
