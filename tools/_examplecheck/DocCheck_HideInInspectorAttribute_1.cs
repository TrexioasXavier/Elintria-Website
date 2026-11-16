namespace DocCheck_HideInInspectorAttribute
{
using Elintria;

public class Projectile2 : ElintriaScript
{
    public float speed = 20f;
    [HideInInspector] public GameObject shooter;   // set by the gun that fires it

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == shooter) return;
        other.gameObject.SendMessage("ApplyDamage", 10f);
        Destroy(gameObject);
    }

    public override void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
}
