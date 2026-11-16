namespace DocCheck_Agent
{
using Elintria;
using Elintria.ML;

public class LaneRunner : Agent
{
    public GameObject obstacle;
    int lane;

    public override int ContinuousActions => 0;
    public override int[] DiscreteBranches => new[] { 3 };   // 0 left, 1 stay, 2 right

    public override void OnEpisodeBegin() { lane = 0; obstacle.transform.position = new Vector3(Random.Range(-1, 2) * 2f, 0.5f, 20f); }

    public override void CollectObservations(Observations o)
    {
        o.AddOneHot(lane + 1, 3);
        o.Add(obstacle.transform.position.x / 2f);
        o.Add(obstacle.transform.position.z / 20f);
    }

    public override void OnActionReceived(ActionBuffers a)
    {
        lane = Mathf.Clamp(lane + a.discrete[0] - 1, -1, 1);
        transform.position = new Vector3(lane * 2f, 0.5f, 0f);
        obstacle.transform.position += Vector3.back * 0.5f;
        if (obstacle.transform.position.z < 0.5f)
        {
            bool hit = Mathf.Abs(obstacle.transform.position.x - transform.position.x) < 1f;
            AddReward(hit ? -1f : 1f);
            EndEpisode();
        }
    }
}
}
