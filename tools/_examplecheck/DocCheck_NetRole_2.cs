namespace DocCheck_NetRole
{
using Elintria;

public class OfflineOnly : ElintriaScript
{
    public override void Update()
    {
        if (NetworkSession.Role == NetRole.Offline && Input.GetKeyDown(KeyCode.P)) Time.timeScale = Time.timeScale == 0f ? 1f : 0f;   // pausing only offline
    }
}
}
