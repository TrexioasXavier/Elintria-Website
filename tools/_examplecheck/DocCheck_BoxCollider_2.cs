namespace DocCheck_BoxCollider
{
using Elintria;

public class InvisibleWalls : ElintriaScript
{
    public float arenaSize = 30f;

    public override void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject wall = new GameObject("Wall " + i);
            float a = i * 90f * Mathf.Deg2Rad;
            wall.transform.position = new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a)) * arenaSize * 0.5f + Vector3.up * 5f;
            BoxCollider b = wall.AddComponent<BoxCollider>();
            b.size = (i % 2 == 0) ? new Vector3(arenaSize, 10f, 1f) : new Vector3(1f, 10f, arenaSize);
        }
    }
}
}
