namespace DocCheck_StdDamage
{
using Elintria;
using Elintria.Standard;

public class ZapGun : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 40f))
            if (!StdDamage.Apply(hit.gameObject, 25f, gameObject, hit.point, transform.forward, 3f))
                Debug.Log("Hit " + hit.gameObject.name + ", which can't be hurt");
    }
}
}
