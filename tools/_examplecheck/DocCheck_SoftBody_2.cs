namespace DocCheck_SoftBody
{
using Elintria;

public class Cushion : ElintriaScript
{
    public override void Update()
    {
        SoftBody s = GetComponent<SoftBody>();
        if (s != null && s.volume < 0.85f) Debug.Log("Someone is sitting on " + gameObject.name);
    }
}
}
