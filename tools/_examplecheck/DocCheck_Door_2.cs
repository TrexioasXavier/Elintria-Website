namespace DocCheck_Door
{
using Elintria;
using Elintria.Standard;

public class EmergencyRelease : ElintriaScript
{
    public void Release()
    {
        foreach (Door d in FindObjectsOfType<Door>())
        {
            d.Unlock();
            d.Open();
        }
        Debug.Log("All doors released");
    }
}
}
