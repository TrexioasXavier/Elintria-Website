namespace DocCheck_ManagedComponent
{
using Elintria;

public class PathCache : ManagedComponent
{
    public Vector3[] corners = new Vector3[0];
    public float computedAt = -1f;
}

public class Pathing : ElintriaScript
{
    public static Vector3[] PathFor(GameObject who, Vector3 goal)
    {
        PathCache cache = who.GetComponent<PathCache>() ?? who.AddComponent<PathCache>();
        if (Time.time - cache.computedAt > 1f)
        {
            NavAgent.CalculatePath(who.transform.position, goal, out cache.corners);
            cache.computedAt = Time.time;
        }
        return cache.corners;
    }
}
}
