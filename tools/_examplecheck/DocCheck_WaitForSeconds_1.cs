namespace DocCheck_WaitForSeconds
{
using System.Collections;
using Elintria;

public class Regenerate : ElintriaScript
{
    public float health = 50f;
    public float maxHealth = 100f;
    readonly WaitForSeconds tick = new WaitForSeconds(0.5f);

    public override void Start() { StartCoroutine(Regen()); }

    IEnumerator Regen()
    {
        while (true)
        {
            health = Mathf.Min(maxHealth, health + 2f);
            yield return tick;
        }
    }
}
}
