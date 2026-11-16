namespace DocCheck_NavMeshRuntime
{
using Elintria;

public class BuildWall : ElintriaScript
{
    int obstacle = -1;

    public override void Start()
    {
        obstacle = NavMeshRuntime.AddBoxObstacle(transform.position, new Vector3(4f, 2.5f, 0.3f), transform.eulerAngles.y, true);
    }

    public override void OnDestroy()
    {
        if (obstacle >= 0) NavMeshRuntime.RemoveObstacle(obstacle);
    }
}
}
