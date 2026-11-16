namespace DocCheck_Spline
{
using Elintria;

public class FencePosts : ElintriaScript
{
    public GameObject path;
    public GameObject postTemplate;
    public float spacing = 2f;

    public override void Start()
    {
        Spline s = Spline.Get(path);
        for (float d = 0f; d <= s.length; d += spacing)
        {
            Vector3 side = Vector3.Cross(Vector3.up, s.TangentAtDistance(d)).normalized * 3f;
            Instantiate(postTemplate, s.EvaluateAtDistance(d) + side, Quaternion.identity).SetActive(true);
        }
    }
}
}
