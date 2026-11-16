namespace DocCheck_Animator
{
using Elintria;

public class ComboAttack : ElintriaScript
{
    static readonly int AttackId = Animator.StringToHash("Attack");
    Animator anim;

    public override void Start() { anim = GetComponent<Animator>(); }

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        AnimatorStateInfo s = anim.GetCurrentAnimatorStateInfo(0);
        bool canChain = s.IsName("Attack1") && s.normalizedTime > 0.5f;
        if (canChain || !anim.IsInTransition(0)) anim.SetTrigger(AttackId);
    }
}
}
