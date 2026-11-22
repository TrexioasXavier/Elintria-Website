namespace DocCheck_CompoundCollider
{
using Elintria;

public class TableBuilder : ElintriaScript
{
    public override void Start()
    {
        Part("Top", new Vector3(0f, 0f, 0f), new Vector3(2f, 0.15f, 1.2f));
        Part("Leg 1", new Vector3(-0.9f, -0.5f, -0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 2", new Vector3(0.9f, -0.5f, -0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 3", new Vector3(-0.9f, -0.5f, 0.5f), new Vector3(0.12f, 1f, 0.12f));
        Part("Leg 4", new Vector3(0.9f, -0.5f, 0.5f), new Vector3(0.12f, 1f, 0.12f));
        gameObject.AddComponent<Rigidbody>().mass = 15f;
        gameObject.AddComponent<CompoundCollider>();
    }

    void Part(string name, Vector3 offset, Vector3 size)
    {
        GameObject p = GameObject.Create(name);
        p.transform.position = transform.position + offset;
        p.transform.parent = transform;
        p.AddComponent<BoxCollider>().size = size;
    }
}
}
