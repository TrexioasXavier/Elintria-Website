namespace DocCheck_StdMotor
{
using Elintria;
using Elintria.Standard;

public class Blink : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(StdMotor.Key("B")) || StdMotor.IsExternal(gameObject)) return;
        if (!StdMotor.Spend(gameObject, 30f)) return;
        transform.position += StdMotor.Facing(gameObject) * 5f;
    }
}
}
