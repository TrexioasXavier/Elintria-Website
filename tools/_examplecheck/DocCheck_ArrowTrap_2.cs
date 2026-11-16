namespace DocCheck_ArrowTrap
{
using Elintria;
using Elintria.Standard;

public class TurningTrap : ElintriaScript
{
    public override void Update()
    {
        transform.eulerAngles = new Vector3(0f, Mathf.Sin(Time.time * 0.5f) * 45f, 0f);
    }
}
}
