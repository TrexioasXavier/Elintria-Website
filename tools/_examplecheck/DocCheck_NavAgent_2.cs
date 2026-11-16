namespace DocCheck_NavAgent
{
using Elintria;

public class ReachableSpawn : ElintriaScript
{
    public GameObject player;

    public bool CanReachPlayer(Vector3 from)
    {
        if (!NavAgent.hasMesh) return false;
        if (!NavAgent.SamplePosition(from, 2f, out Vector3 onMesh)) return false;
        return NavAgent.CalculatePath(onMesh, player.transform.position, out Vector3[] corners) && corners.Length > 0;
    }
}
}
