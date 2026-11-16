namespace DocCheck_Plane
{
using Elintria;

public class SimpleWaterline : ElintriaScript
{
    public float waterHeight = 0f;

    public override void Update()
    {
        Plane water = new Plane(Vector3.up, new Vector3(0f, waterHeight, 0f));   // faces up, at waterHeight
        float depth = -water.GetDistanceToPoint(transform.position);
        if (depth > 0f) transform.position += Vector3.up * Mathf.Min(depth, 2f * Time.deltaTime);
    }
}
}
