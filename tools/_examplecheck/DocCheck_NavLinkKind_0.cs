namespace DocCheck_NavLinkKind
{
using Elintria;

public class LinkAnims : ElintriaScript
{
    public override void Update()
    {
        NavAgent a = NavAgent.Of(gameObject);
        Animator anim = GetComponent<Animator>();
        if (anim == null) return;
        anim.SetBool("Climbing", a.currentLinkKind == NavLinkKind.Climb);
        anim.SetBool("Jumping", a.currentLinkKind == NavLinkKind.Jump || a.currentLinkKind == NavLinkKind.Drop);
    }
}
}
