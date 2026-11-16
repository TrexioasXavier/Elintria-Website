namespace DocCheck_GameObject
{
using Elintria;

public class GameDirector : ElintriaScript
{
    GameObject player;
    GameObject[] enemies;
    GameObject[] spawnPoints;

    public override void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        spawnPoints = GameObject.FindObjectsWithNamePrefix("Spawn_");
        GameObject exit = GameObject.Find("Exit Door");

        Debug.Log("Player: " + (player != null ? player.name : "none") +
                  ", enemies: " + enemies.Length +
                  ", spawn points: " + spawnPoints.Length +
                  ", exit found: " + (exit != null));
    }

    public void RespawnPlayer()
    {
        if (spawnPoints.Length == 0 || !player) return;
        GameObject p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        player.transform.position = p.transform.position;
    }
}
}
