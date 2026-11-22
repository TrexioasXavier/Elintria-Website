namespace DocCheck_SoftBody
{
using Elintria;

public class JellyCube : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Box;
        s.resolution = 4;
        s.shapeMatching = 0.35f;
        s.bounciness = 0.4f;
    }
}
}
