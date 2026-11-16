namespace DocCheck_WaitWhile
{
using System.Collections;
using Elintria;

public class Landed : ElintriaScript
{
    public override void Start() { StartCoroutine(Run()); }

    IEnumerator Run()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.2f);
        yield return new WaitWhile(() => rb.velocity.y < -0.1f);
        Debug.Log("Landed");
    }
}
}
