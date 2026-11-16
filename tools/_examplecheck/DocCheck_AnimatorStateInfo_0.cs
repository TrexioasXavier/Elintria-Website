namespace DocCheck_AnimatorStateInfo
{
using System.Collections;
using Elintria;

public class WaitForAnim : ElintriaScript
{
    public IEnumerator PlayAndWait(string state)
    {
        Animator a = GetComponent<Animator>();
        a.Play(state);
        yield return null;
        yield return new WaitUntil(() => !a.GetCurrentAnimatorStateInfo(0).IsName(state) || a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}
}
