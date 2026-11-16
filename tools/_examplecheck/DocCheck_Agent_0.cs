namespace DocCheck_Agent
{
using Elintria;
using Elintria.ML;

public class Roller : Agent
{
    public GameObject target;
    Rigidbody rb;

    public override int ContinuousActions => 2;

    public override void Start() { rb = GetComponent<Rigidbody>(); }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0f, 0.5f, 0f);
        target.transform.position = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
    }

    public override void CollectObservations(Observations o)
    {
        o.Add(target.transform.position - transform.position);   // 3 numbers
        o.Add(rb.velocity.x);
        o.Add(rb.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        rb.AddForce(new Vector3(a.continuous[0], 0f, a.continuous[1]) * 10f);
        float d = Vector3.Distance(transform.position, target.transform.position);
        if (d < 1f) { AddReward(1f); EndEpisode(); }
        else if (transform.position.y < -1f) { AddReward(-1f); EndEpisode(); }
        else AddReward(-0.001f);   // hurry up
    }

    public override void Heuristic(ActionBuffers a)
    {
        a.continuous[0] = Input.GetAxisHorizontal();
        a.continuous[1] = Input.GetAxisVertical();
    }
}
}
