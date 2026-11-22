namespace DocCheck_CompassBar
{
using Elintria;
using Elintria.Standard;

public class StealthCompass : ElintriaScript
{
    public override void Start()
    {
        CompassBar c = GetComponent<CompassBar>();
        c.onlyAlerted = false;        // every enemy near shows
        c.enemyRange = 20f;
        c.showHeading = false;
    }
}
}
