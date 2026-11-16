namespace DocCheck_PlayerRespawn
{
using Elintria;
using Elintria.Standard;

public class ThreeLives : ElintriaScript
{
    public string gameOverScene = "Scenes/Menu.scene";

    public void OnRespawn()
    {
        if (GetComponent<PlayerRespawn>().deaths >= 3) SceneManager.LoadScene(gameOverScene);
    }
}
}
