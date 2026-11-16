namespace DocCheck_SlideAbility
{
using Elintria;
using Elintria.Standard;

public class SlopeSlide : ElintriaScript
{
    public override void Update()
    {
        SlideAbility s = GetComponent<SlideAbility>();
        s.duration = transform.position.y > 5f ? 1.4f : 0.8f;
    }
}
}
