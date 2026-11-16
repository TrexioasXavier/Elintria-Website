namespace DocCheck_Destructible
{
using Elintria;
using Elintria.Standard;

public class LootCrate : ElintriaScript
{
    public GameObject lootTemplate;

    public override void Start()
    {
        Destructible d = GetComponent<Destructible>();
        d.health = 30f;
        d.pieces = 8;
        d.pieceLifetime = 6f;
        d.onBreakTargets.Add(gameObject);
        d.onBreakMessage = "DropLoot";
    }

    public void DropLoot()
    {
        Instantiate(lootTemplate, transform.position + Vector3.up * 0.3f, Quaternion.identity).SetActive(true);
    }
}
}
