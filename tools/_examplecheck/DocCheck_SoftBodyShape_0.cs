namespace DocCheck_SoftBodyShape
{
using Elintria;

public class SoftBall : ElintriaScript
{
    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = SoftBodyShape.Sphere;
        s.size = new Vector3(1f, 1f, 1f);
        s.bounciness = 0.7f;
    }
}
}
