namespace DocCheck_Animator
{
using System.Collections;
using Elintria;

public class NpcActing : ElintriaScript
{
    public override void Start() { StartCoroutine(Act()); }

    IEnumerator Act()
    {
        Animator a = GetComponent<Animator>();
        a.Play("Sit");
        yield return new WaitForSeconds(3f);
        a.CrossFadeInFixedTime("StandUp", 0.25f);
        yield return new WaitForSeconds(1.5f);
        a.speed = 1.2f;
        a.CrossFade("Wave", 0.2f);
    }
}
}
