namespace DocCheck_Elevator
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class ElevatorButtons : ElintriaScript
{
    public GameObject elevator;
    public List<GameObject> floorLevers = new List<GameObject>();

    public override void Start()
    {
        for (int i = 0; i < floorLevers.Count; i++)
        {
            Lever l = floorLevers[i].GetComponent<Lever>();
            l.onOnTargets.Add(elevator);
            l.onOnMessage = "GoToFloor:" + i;
        }
    }
}
}
