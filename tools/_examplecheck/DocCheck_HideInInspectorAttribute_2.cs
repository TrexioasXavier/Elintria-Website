namespace DocCheck_HideInInspectorAttribute
{
using Elintria;

public class Cached : ElintriaScript
{
    [HideInInspector] public Rigidbody body;

    public override void Awake() { body = GetComponent<Rigidbody>(); }
}
}
