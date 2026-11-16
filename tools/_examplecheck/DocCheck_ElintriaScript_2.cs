namespace DocCheck_ElintriaScript
{
using Elintria;

public class RockSpawner : ElintriaScript
{
    public GameObject rockTemplate;   // an inactive rock in the scene
    public float every = 2f;
    public float lifetime = 10f;

    public override void OnEnable()
    {
        InvokeRepeating("Spawn", 0.5f, every);
    }

    public override void OnDisable()
    {
        CancelInvoke("Spawn");
    }

    void Spawn()
    {
        Vector3 at = transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        GameObject rock = Instantiate(rockTemplate, at, Random.rotation);
        rock.SetActive(true);
        Destroy(rock, lifetime);
    }
}
}
