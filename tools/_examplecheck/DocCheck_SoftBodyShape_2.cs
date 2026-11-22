namespace DocCheck_SoftBodyShape
{
using Elintria;

public class SoftShapePicker : ElintriaScript
{
    public bool round;

    public override void Start()
    {
        SoftBody s = gameObject.AddComponent<SoftBody>();
        s.shape = round ? SoftBodyShape.Sphere : SoftBodyShape.Box;
    }
}
}
