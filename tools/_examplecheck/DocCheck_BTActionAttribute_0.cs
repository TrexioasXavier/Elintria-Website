namespace DocCheck_BTActionAttribute
{
using Elintria;
using Elintria.AI;

public class WorkerActions : ElintriaScript
{
    [BTAction] BTStatus ChopTree() { return BTStatus.Success; }
    [BTAction] void Rest() { }
}
}
