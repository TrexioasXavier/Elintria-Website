namespace DocCheck_StdEvent
{
using Elintria;
using Elintria.Standard;

public class EventValues : ElintriaScript
{
    public GameObject target;

    public override void Start()
    {
        StdEvent.Fire(target, "SetSpeed:4.5");     // SetSpeed(float)
        StdEvent.Fire(target, "GoToFloor:2");      // GoToFloor(int)
        StdEvent.Fire(target, "SetActive:false");  // no script needed
    }
}

public class Receiver : ElintriaScript
{
    public void SetSpeed(float s) { Debug.Log("speed " + s); }
    public void GoToFloor(int f) { Debug.Log("floor " + f); }
}
}
