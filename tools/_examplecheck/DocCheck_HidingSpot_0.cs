namespace DocCheck_HidingSpot
{
using Elintria;
using Elintria.Standard;

public class RestingLocker : ElintriaScript
{
    public override void Update()
    {
        HidingSpot spot = GetComponent<HidingSpot>();
        if (!spot.occupied) return;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) p.GetComponent<Health>().Heal(2f * Time.deltaTime);
    }
}
}
