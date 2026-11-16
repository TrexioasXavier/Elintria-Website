namespace DocCheck_MeshCollider
{
using Elintria;

public class SolidScenery : ElintriaScript
{
    public override void Start()
    {
        foreach (Transform child in transform)
            if (child.GetComponent<Collider>() == null)
                child.gameObject.AddComponent<MeshCollider>();
    }
}
}
