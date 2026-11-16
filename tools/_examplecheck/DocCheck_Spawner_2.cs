namespace DocCheck_Spawner
{
using Elintria;
using Elintria.Standard;

public class SpawnedEnemy : ElintriaScript
{
    public void OnSpawned(Spawner from)
    {
        Health h = GetComponent<Health>();
        if (h != null) h.maxHealth = h.health = 50f + from.spawned * 5f;   // later ones are tougher
    }
}
}
