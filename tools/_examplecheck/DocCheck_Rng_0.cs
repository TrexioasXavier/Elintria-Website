namespace DocCheck_Rng
{
using Elintria;
using Elintria.Procedural;

public class SeededLevel : ElintriaScript
{
    public int seed = 1234;
    public GameObject crateTemplate;

    public override void Start()
    {
        var rng = new Rng(seed);
        int crates = rng.Range(5, 12);
        for (int i = 0; i < crates; i++)
        {
            Vector2 p = rng.InsideUnitCircle() * 20f;
            GameObject c = Instantiate(crateTemplate, new Vector3(p.x, 0.5f, p.y), Quaternion.Euler(0f, rng.Range(0f, 360f), 0f));
            c.SetActive(true);
        }
    }
}
}
