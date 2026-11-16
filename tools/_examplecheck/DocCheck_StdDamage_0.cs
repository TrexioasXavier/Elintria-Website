namespace DocCheck_StdDamage
{
using Elintria;
using Elintria.Standard;

public class AxeTrap : ElintriaScript
{
    public float damage = 40f;

    public override void Update()
    {
        transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time * 2f) * 70f);
    }

    public override void OnTriggerEnter(Collider other)
    {
        Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
        StdDamage.Apply(other.gameObject, damage, gameObject, other.ClosestPoint(transform.position), dir, 8f);
    }
}
}
