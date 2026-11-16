namespace DocCheck_ForceMode
{
using Elintria;

public class ImpulseJump : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
}
