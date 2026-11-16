namespace DocCheck_ComponentBase
{
using Elintria;

public class BodyWatcher : ElintriaScript
{
    public GameObject crate;
    Rigidbody body;

    public override void Start()
    {
        if (crate != null) body = crate.GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        if (body == null || !body.IsAlive) return;   // the crate was destroyed
        if (body.velocity.magnitude > 10f)
            Debug.Log(body.gameObject.name + " is flying!");
    }
}
}
