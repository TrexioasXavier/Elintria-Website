namespace DocCheck_CompassBar
{
using Elintria;
using Elintria.Standard;

public class NorthDoor : ElintriaScript
{
    public Door door;

    public override void Update()
    {
        float h = CompassBar.Heading();
        if ((h < 5f || h > 355f) && door.locked) { door.Unlock(); door.Open(); }
    }
}
}
