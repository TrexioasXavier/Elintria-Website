namespace DocCheck_StealthPlayer
{
using Elintria;
using Elintria.Standard;

public class ThrowBottle : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;
        Vector3 landing = transform.position + transform.forward * 10f;
        StdNoise.Make(landing, 15f, gameObject);     // guards go and look there
    }
}
}
