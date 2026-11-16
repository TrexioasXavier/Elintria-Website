namespace DocCheck_SphereCollider
{
using Elintria;

public class AggroRange : ElintriaScript
{
    public float calmRadius = 6f;
    public float alertRadius = 14f;
    public bool alerted;

    public override void Update()
    {
        SphereCollider s = GetComponent<SphereCollider>();
        if (s != null) s.radius = Mathf.MoveTowards(s.radius, alerted ? alertRadius : calmRadius, 4f * Time.deltaTime);
    }
}
}
