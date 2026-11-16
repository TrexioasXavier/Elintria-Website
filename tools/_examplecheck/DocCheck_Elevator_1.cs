namespace DocCheck_Elevator
{
using Elintria;
using Elintria.Standard;

public class TowerLift : ElintriaScript
{
    public override void Start()
    {
        Elevator e = GetComponent<Elevator>();
        e.floorHeights.Clear();
        e.floorHeights.Add(0f);
        for (int i = 1; i <= 5; i++) e.floorHeights.Add(i * 4f);   // five floors, 4 m apart
        e.speed = 3f;
        e.returnAfter = 20f;
    }
}
}
