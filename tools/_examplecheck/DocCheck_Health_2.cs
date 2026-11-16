namespace DocCheck_Health
{
using Elintria;
using Elintria.Standard;

public class AcidPool : ElintriaScript
{
    public float damagePerSecond = 15f;

    public override void OnTriggerStay(Collider other)
    {
        Health h = other.gameObject.GetComponent<Health>();
        if (h != null && h.Damage(damagePerSecond * Time.fixedDeltaTime, gameObject))
            Debug.Log(other.gameObject.name + " burns (" + h.health.ToString("0") + " left)");
    }
}

public class MedKit : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        Health h = other.gameObject.GetComponent<Health>();
        if (h == null || h.health >= h.maxHealth) return;
        h.Heal(50f);
        Destroy(gameObject);
    }
}
}
