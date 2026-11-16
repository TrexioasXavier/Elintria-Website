namespace DocCheck_Physics
{
using Elintria;

public class PlasmaBolt : ElintriaScript
{
    public float speed = 40f;
    public float radius = 0.2f;

    public override void Update()
    {
        float step = speed * Time.deltaTime;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, step))
        {
            hit.gameObject.SendMessage("ApplyDamage", 25f);
            Destroy(gameObject);
            return;
        }
        transform.position += transform.forward * step;
    }
}
}
