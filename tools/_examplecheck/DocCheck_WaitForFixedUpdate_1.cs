namespace DocCheck_WaitForFixedUpdate
{
using System.Collections;
using Elintria;

public class SettleCheck : ElintriaScript
{
    public override void Start() { StartCoroutine(Wait()); }

    IEnumerator Wait()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        do { yield return new WaitForFixedUpdate(); } while (rb.velocity.magnitude > 0.05f);
        Debug.Log(name + " came to rest at " + transform.position);
    }
}
}
