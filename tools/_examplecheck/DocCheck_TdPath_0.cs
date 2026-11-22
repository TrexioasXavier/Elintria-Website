namespace DocCheck_TdPath
{
using Elintria;
using Elintria.Standard;

public class HalfwayMarker : ElintriaScript
{
    public override void Start()
    {
        TdPath p = FindObjectOfType<TdPath>();
        Vector3 dir;
        transform.position = p.At(p.length * 0.5f, out dir);
    }
}
}
