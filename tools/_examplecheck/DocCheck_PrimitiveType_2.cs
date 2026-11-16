namespace DocCheck_PrimitiveType
{
using Elintria;

public class QuickBullet : ElintriaScript
{
    public static GameObject Fire(Vector3 from, Vector3 dir, float speed)
    {
        GameObject b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        b.transform.position = from;
        b.transform.localScale = Vector3.one * 0.15f;
        Rigidbody rb = b.AddComponent<Rigidbody>();
        rb.velocity = dir.normalized * speed;
        ElintriaScript.Destroy(b, 4f);
        return b;
    }
}
}
