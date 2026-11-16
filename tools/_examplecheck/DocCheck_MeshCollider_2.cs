namespace DocCheck_MeshCollider
{
using Elintria;

public class ColliderReport : ElintriaScript
{
    public override void Start()
    {
        Collider c = GetComponent<Collider>();
        if (c is MeshCollider m) Debug.Log(name + ": mesh collider, convex=" + m.convex);
        else if (c is BoxCollider b) Debug.Log(name + ": box " + b.size);
        else if (c is SphereCollider s) Debug.Log(name + ": sphere r=" + s.radius);
    }
}
}
