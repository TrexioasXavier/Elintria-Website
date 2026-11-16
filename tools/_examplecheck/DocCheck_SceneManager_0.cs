namespace DocCheck_SceneManager
{
using Elintria;

public class LevelExit : ElintriaScript
{
    public string nextScene = "Level2";
    public string spawnPoint = "Spawn_FromLevel1";

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(nextScene, spawnPoint);
    }
}
}
