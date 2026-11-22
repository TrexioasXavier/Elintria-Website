namespace DocCheck_CompoundCollider
{
using Elintria;

public class CompoundReport : ElintriaScript
{
    float t;

    public override void Update()
    {
        t += Time.deltaTime;
        if (t < 0.5f) return;
        CompoundCollider c = GetComponent<CompoundCollider>();
        Debug.Log(gameObject.name + " is " + c.partCount + " shapes");
        enabled = false;
    }
}
}
