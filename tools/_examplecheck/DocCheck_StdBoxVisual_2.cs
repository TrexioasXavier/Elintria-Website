namespace DocCheck_StdBoxVisual
{
using Elintria;
using Elintria.Standard;

public class PulseBox : ElintriaScript
{
    public override void Start()
    {
        StdBoxVisual v = gameObject.AddComponent<StdBoxVisual>();
        v.color = new Color(1f, 0.2f, 0.2f, 1f);
        v.size = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
}
