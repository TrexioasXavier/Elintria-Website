namespace DocCheck_PrimitiveType
{
using Elintria;

public class ShapeSpawner : ElintriaScript
{
    public int shape = 3;   // 0 Sphere, 1 Capsule, 2 Cylinder, 3 Cube, 4 Plane, 5 Quad

    public override void Start()
    {
        GameObject g = GameObject.CreatePrimitive((PrimitiveType)Mathf.Clamp(shape, 0, 5));
        g.transform.position = transform.position;
    }
}
}
