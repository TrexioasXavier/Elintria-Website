namespace DocCheck_ComponentBase
{
using Elintria;

public class MakeDynamic : ElintriaScript
{
    public override void Start()
    {
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(1f, 2f, 1f);

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 5f;
        rb.useGravity = true;
        Debug.Log("Added " + box + " and " + rb);
    }
}
}
