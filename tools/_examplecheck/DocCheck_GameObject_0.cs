namespace DocCheck_GameObject
{
using Elintria;

public class ArenaBuilder : ElintriaScript
{
    public int pillars = 8;
    public float radius = 10f;

    public override void Start()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Arena Floor";
        floor.transform.localScale = new Vector3(radius * 2.5f, 0.5f, radius * 2.5f);

        for (int i = 0; i < pillars; i++)
        {
            float a = i * Mathf.PI * 2f / pillars;
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            p.name = "Pillar " + i;
            p.transform.position = new Vector3(Mathf.Cos(a) * radius, 2f, Mathf.Sin(a) * radius);
            p.transform.localScale = new Vector3(1f, 4f, 1f);
        }

        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.transform.position = new Vector3(0f, 8f, 0f);
        ball.AddComponent<Rigidbody>().mass = 2f;
    }
}
}
