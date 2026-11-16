namespace DocCheck_Physics
{
using Elintria;

public class Railgun : ElintriaScript
{
    public int maxPierce = 3;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 100f);
        for (int i = 0; i < hits.Length && i < maxPierce; i++)
        {
            hits[i].gameObject.SendMessage("ApplyDamage", 50f / (i + 1));
            Debug.DrawLine(transform.position, hits[i].point, Color.cyan, 0.3f);
        }
    }
}
}
