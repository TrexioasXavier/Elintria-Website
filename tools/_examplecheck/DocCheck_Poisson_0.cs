namespace DocCheck_Poisson
{
using Elintria;
using Elintria.Procedural;

public class Forest : ElintriaScript
{
    public GameObject treeTemplate;
    public float size = 80f;
    public float spacing = 4f;

    public override void Start()
    {
        foreach (Vector2 p in Poisson.Sample(size, size, spacing, 99))
        {
            Vector3 at = transform.position + new Vector3(p.x - size / 2f, 0f, p.y - size / 2f);
            Instantiate(treeTemplate, at, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)).SetActive(true);
        }
    }
}
}
