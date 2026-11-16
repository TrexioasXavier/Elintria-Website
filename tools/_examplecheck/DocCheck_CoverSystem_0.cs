namespace DocCheck_CoverSystem
{
using Elintria;
using Elintria.Standard;

public class CoverRest : ElintriaScript
{
    public override void Update()
    {
        if (GetComponent<CoverSystem>().inCover) GetComponent<Health>().Heal(3f * Time.deltaTime);
    }
}
}
