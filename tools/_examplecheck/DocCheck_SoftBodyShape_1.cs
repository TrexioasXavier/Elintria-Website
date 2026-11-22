namespace DocCheck_SoftBodyShape
{
using Elintria;

public class SoftModel : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Model;
        s.meshPath = "Assets/Models/Blob.obj";
    }
}
}
