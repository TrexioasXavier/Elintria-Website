namespace DocCheck_ConvexCollider
{
using Elintria;

public class TippingCrate : ElintriaScript
{
    public override void Start()
    {
        ConvexCollider box = gameObject.AddComponent<ConvexCollider>();
        box.shape = ConvexShape.Box;
        box.size = Vector3.one;
        gameObject.AddComponent<Rigidbody>().mass = 5f;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 1f, 4f), ForceMode.Impulse);
    }
}
}
