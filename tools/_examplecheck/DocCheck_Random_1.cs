namespace DocCheck_Random
{
using Elintria;

public class Scatter : ElintriaScript
{
    public GameObject rockTemplate;
    public int count = 30;
    public float radius = 15f;

    public override void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 c = Random.insideUnitCircle * radius;
            GameObject r = Instantiate(rockTemplate, transform.position + new Vector3(c.x, 0f, c.y), Random.rotation);
            r.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            r.SetActive(true);
        }
    }
}
}
