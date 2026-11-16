namespace DocCheck_Collision
{
using Elintria;

public class Sparks : ElintriaScript
{
    public GameObject sparkTemplate;

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.contactCount == 0) return;
        ContactPoint c = collision.GetContact(0);
        GameObject s = Instantiate(sparkTemplate, c.point, Quaternion.LookRotation(c.normal));
        s.SetActive(true);
        Destroy(s, 1f);
    }
}
}
