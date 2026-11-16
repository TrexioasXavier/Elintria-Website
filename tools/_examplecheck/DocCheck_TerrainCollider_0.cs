namespace DocCheck_TerrainCollider
{
using Elintria;

public class TerrainHitCheck : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f))
            Debug.Log(hit.collider is TerrainCollider ? "Hit the ground" : "Hit " + hit.gameObject.name);
    }
}
}
