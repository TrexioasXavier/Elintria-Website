namespace DocCheck_ContactPoint
{
using Elintria;

public class Decals : ElintriaScript
{
    public GameObject decalTemplate;

    public override void OnCollisionEnter(Collision collision)
    {
        ContactPoint p = collision.GetContact(0);
        GameObject d = Instantiate(decalTemplate, p.point + p.normal * 0.01f, Quaternion.LookRotation(-p.normal));
        d.SetActive(true);
    }
}
}
