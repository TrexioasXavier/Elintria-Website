namespace DocCheck_ConveyorBelt
{
using Elintria;
using Elintria.Standard;

public class BeltSwitch : ElintriaScript
{
    public ConveyorBelt belt;
    public void Pressed() { belt.SetRunning(true); }
    public void Released() { belt.SetRunning(false); }
}
}
