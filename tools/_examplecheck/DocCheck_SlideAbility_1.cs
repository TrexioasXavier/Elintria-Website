namespace DocCheck_SlideAbility
{
using Elintria;
using Elintria.Standard;

public class SlideKick : ElintriaScript
{
    public override void Update()
    {
        if (!GetComponent<SlideAbility>().sliding) return;
        foreach (BasicEnemy e in FindObjectsOfType<BasicEnemy>())
            if (Vector3.Distance(e.transform.position, transform.position) < 1.2f)
                StdDamage.Apply(e.gameObject, 5f, gameObject, e.transform.position, transform.forward, 6f);
    }
}
}
