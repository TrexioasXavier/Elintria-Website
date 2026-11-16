namespace DocCheck_HitFeedback
{
using Elintria;
using Elintria.Standard;

public class ComfortShake : ElintriaScript
{
    public bool reduceMotion = true;

    public override void Start()
    {
        HitFeedback h = GetComponent<HitFeedback>();
        if (reduceMotion) { h.maxShake = 0.05f; h.hitStop = 0f; }
    }
}
}
