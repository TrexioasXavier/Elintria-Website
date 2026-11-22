namespace DocCheck_WindZone
{
using Elintria;
using Elintria.Standard;

public class WindSock : ElintriaScript
{
    public override void Update()
    {
        Vector3 w = WindZone.At(transform.position);
        if (new Vector3(w.x, 0f, w.z).sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(new Vector3(w.x, 0f, w.z));
    }
}
}
