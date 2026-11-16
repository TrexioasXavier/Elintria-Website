namespace DocCheck_Mount
{
using Elintria;
using Elintria.Standard;

public class RoadSpeed : ElintriaScript
{
    public float roadHalfWidth = 3f;

    public override void Update()
    {
        Mount m = GetComponent<Mount>();
        m.gallopSpeed = Mathf.Abs(transform.position.x) < roadHalfWidth ? 14f : 10f;
    }
}
}
