namespace DocCheck_GrassBender
{
using Elintria;
using Elintria.Standard;

public class RunBend : ElintriaScript
{
    public override void Update()
    {
        GetComponent<GrassBender>().radius = Input.GetKey(KeyCode.Shift) ? 1.2f : 0.7f;
    }
}
}
