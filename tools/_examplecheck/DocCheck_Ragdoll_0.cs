namespace DocCheck_Ragdoll
{
using System.Collections;
using Elintria;
using Elintria.Standard;

public class Knockdown : ElintriaScript
{
    public void HeavyHit(Vector3 dir) { StartCoroutine(Down(dir)); }

    IEnumerator Down(Vector3 dir)
    {
        Ragdoll r = GetComponent<Ragdoll>();
        r.Activate();
        r.AddImpulse(dir.normalized * 8f + Vector3.up * 3f, transform.position + Vector3.up);
        yield return new WaitUntil(() => r.settled);
        yield return new WaitForSeconds(1f);
        r.GetUp();
    }
}
}
