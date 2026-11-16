namespace DocCheck_FindObjectsInactive
{
using Elintria;

public class PauseScreen : ElintriaScript { }

public class MenuFinder : ElintriaScript
{
    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        PauseScreen menu = FindFirstObjectByType<PauseScreen>(FindObjectsInactive.Include);
        if (menu != null) menu.gameObject.SetActive(!menu.gameObject.activeSelf);
    }
}
}
