namespace DocCheck_SoftBody
{
using Elintria;

public class PokeSoft : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        SoftBody s = GetComponent<SoftBody>();
        Vector3 top = s.centre + Vector3.up * 0.5f;
        s.AddImpulseAt(Vector3.down * 3f, top, 0.4f);
    }
}
}
