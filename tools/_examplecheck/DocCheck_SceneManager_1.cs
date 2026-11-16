namespace DocCheck_SceneManager
{
using Elintria;

public class SpawnAtEntrance : ElintriaScript
{
    public override void Start()
    {
        Debug.Log("Now in " + SceneManager.activeSceneName);
        string spawn = SceneManager.sceneData;
        GameObject point = string.IsNullOrEmpty(spawn) ? null : GameObject.Find(spawn);
        if (point != null) transform.position = point.transform.position;
    }
}
}
