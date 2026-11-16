namespace DocCheck_ForceMode
{
using Elintria;

public class DashBody : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }
}
}
