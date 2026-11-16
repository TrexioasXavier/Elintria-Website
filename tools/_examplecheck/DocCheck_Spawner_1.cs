namespace DocCheck_Spawner
{
using Elintria;
using Elintria.Standard;

public class AmbushSpawner : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        Spawner s = GetComponent<Spawner>();
        for (int i = 0; i < 4; i++) s.SpawnOne();
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) GetComponent<Spawner>().StopSpawning();
    }
}
}
