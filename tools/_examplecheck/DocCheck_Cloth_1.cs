namespace DocCheck_Cloth
{
using Elintria;
using Elintria.Standard;

public class TearableCurtain : ElintriaScript
{
    public override void Start()
    {
        Cloth c = GetComponent<Cloth>();
        c.pinMode = "Top";
        c.tearFactor = 2.5f;
        c.collideWithCharacters = true;
    }

    public void ApplyDamage(float amount)
    {
        Cloth c = GetComponent<Cloth>();
        if (amount > 30f) c.ReleasePins();   // a big hit tears it down
        Debug.Log("Torn links: " + c.brokenLinks);
    }
}
}
