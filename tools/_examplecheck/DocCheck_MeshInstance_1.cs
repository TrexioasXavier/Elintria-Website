namespace DocCheck_MeshInstance
{
using System.Collections.Generic;
using Elintria;

public class InstanceLod : ElintriaScript
{
    public float drawDistance = 60f;
    public readonly List<MeshInstance> instances = new List<MeshInstance>();
    public readonly List<Vector3> positions = new List<Vector3>();

    public override void Update()
    {
        for (int i = 0; i < instances.Count; i++)
            instances[i].visible = Vector3.Distance(positions[i], transform.position) < drawDistance;
    }
}
}
