namespace DocCheck_Debug
{
using Elintria;

public class HealthLogger : ElintriaScript
{
    public float health = 100f;

    public void ApplyDamage(float amount)
    {
        health -= amount;
        Debug.LogFormat("{0} took {1:0.#} damage ({2:0.#} left)", name, amount, health);
        if (health < 25f) Debug.LogWarning(name + " is badly hurt", gameObject);
        if (health <= 0f) Debug.LogError(name + " died");
    }
}
}
