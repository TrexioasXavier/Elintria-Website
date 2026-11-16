namespace DocCheck_RaycastHit
{
using Elintria;

public class Placer : ElintriaScript
{
    public GameObject propTemplate;

    public override void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20f))
        {
            GameObject p = Instantiate(propTemplate, hit.point, Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal));
            p.SetActive(true);
        }
    }
}
}
