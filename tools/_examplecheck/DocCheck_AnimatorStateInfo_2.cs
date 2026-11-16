namespace DocCheck_AnimatorStateInfo
{
using Elintria;

public class HitWindow : ElintriaScript
{
    public bool DamageActive()
    {
        AnimatorStateInfo s = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        return s.IsName("Swing") && s.normalizedTime > 0.35f && s.normalizedTime < 0.55f;
    }
}
}
