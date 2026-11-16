namespace DocCheck_Collider
{
using Elintria;

public class HealingSpring : ElintriaScript
{
    public float healPerSecond = 10f;

    public override void Start() { GetComponent<Collider>().isTrigger = true; }

    public override void OnTriggerStay(Collider other)
    {
        other.gameObject.SendMessage("Heal", healPerSecond * Time.fixedDeltaTime);
    }
}
}
