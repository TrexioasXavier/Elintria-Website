namespace DocCheck_ConveyorBelt
{
using Elintria;
using Elintria.Standard;

public class BeltRamp : ElintriaScript
{
    public override void Update()
    {
        ConveyorBelt b = GetComponent<ConveyorBelt>();
        b.speed = Mathf.Min(8f, b.speed + 0.1f * Time.deltaTime);
    }
}
}
