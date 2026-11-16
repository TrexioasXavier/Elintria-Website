namespace DocCheck_CharacterController
{
using Elintria;

public class CratePusher : ElintriaScript
{
    public float pushPower = 2f;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.rigidbody;
        if (rb == null || rb.isKinematic || hit.moveDirection.y < -0.3f) return;   // not pushing down on it
        Vector3 push = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        rb.velocity = push * pushPower;
    }
}
}
