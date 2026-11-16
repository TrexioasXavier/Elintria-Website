namespace DocCheck_MeleeWeapon
{
using Elintria;
using Elintria.Standard;

public class BruteAttack : ElintriaScript
{
    public GameObject player;

    public override void Update()
    {
        MeleeWeapon m = GetComponent<MeleeWeapon>();
        if (player != null && Vector3.Distance(player.transform.position, transform.position) < m.range)
            m.Swing();
    }
}
}
