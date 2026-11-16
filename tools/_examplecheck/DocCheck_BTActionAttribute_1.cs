namespace DocCheck_BTActionAttribute
{
using Elintria;
using Elintria.AI;

public class NamedActions : ElintriaScript
{
    [BTAction("Throw Grenade")] bool ThrowGrenadeAtTarget() { Debug.Log("Grenade out!"); return true; }
}
}
