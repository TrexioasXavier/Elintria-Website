namespace DocCheck_StdRot
{
using Elintria;
using Elintria.Standard;

public class LimitedTurret : ElintriaScript
{
    public GameObject target;
    Quaternion rest;

    public override void Start() { rest = transform.rotation; }

    public override void Update()
    {
        Quaternion want = Quaternion.LookRotation(target.transform.position - transform.position);
        Quaternion turn = want * Quaternion.Inverse(rest);
        transform.rotation = StdRot.Clamp(turn, 45f) * rest;   // never more than 45 degrees off rest
    }
}
}
