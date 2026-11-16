namespace DocCheck_StdInput
{
using Elintria;
using Elintria.Standard;

public class CutsceneDriver : ElintriaScript
{
    float t;

    public override void Update()
    {
        VehicleController car = GetComponent<VehicleController>();
        car.input.overridden = true;
        t += Time.deltaTime;
        car.input.move = new Vector2(Mathf.Sin(t * 0.5f) * 0.4f, t < 6f ? 1f : 0f);   // weave, then coast
        car.input.handbrake = t > 6f && t < 7f;
        if (t > 9f) car.input.overridden = false;   // hand control back to the player
    }
}
}
