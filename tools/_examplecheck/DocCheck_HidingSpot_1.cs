namespace DocCheck_HidingSpot
{
using Elintria;
using Elintria.Standard;

public class FoundYou : ElintriaScript
{
    public BasicEnemy guard;

    public override void Update()
    {
        HidingSpot spot = GetComponent<HidingSpot>();
        if (spot.occupied && guard != null && Vector3.Distance(guard.transform.position, transform.position) < 1.5f) spot.Leave();
    }
}
}
