namespace DocCheck_LevelObjective
{
using Elintria;

public class OnFinished : ElintriaScript
{
    public void LevelDone()
    {
        Debug.Log("Level finished");
        SceneManager.LoadScene("Level2");
    }
}
}
