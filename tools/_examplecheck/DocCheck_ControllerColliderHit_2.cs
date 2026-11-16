namespace DocCheck_ControllerColliderHit
{
using Elintria;

public class SpikeContact : ElintriaScript
{
    float lastHurt;

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Spikes") && Time.time - lastHurt > 1f)
        {
            lastHurt = Time.time;
            SendMessage("ApplyDamage", 15f);
            Debug.Log("Ouch, at " + hit.point);
        }
    }
}
}
