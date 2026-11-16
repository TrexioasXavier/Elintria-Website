namespace DocCheck_ClimbableSurface
{
using Elintria;
using Elintria.Standard;

public class MakeClimbable : ElintriaScript
{
    public override void Start()
    {
        ClimbableSurface s = gameObject.AddComponent<ClimbableSurface>();
        s.size = new Vector3(3f, 8f, 0.6f);
    }
}
}
