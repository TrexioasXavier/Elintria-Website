namespace DocCheck_ControllerColliderHit
{
using Elintria;

public class BouncePadUser : ElintriaScript
{
    public float launchSpeed = 15f;
    public float verticalSpeed;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("BouncePad") && hit.normal.y > 0.7f)
            verticalSpeed = launchSpeed;   // a motor script reads this
    }
}
}
