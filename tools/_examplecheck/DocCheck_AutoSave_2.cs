namespace DocCheck_AutoSave
{
using Elintria;
using Elintria.Standard;

public class BossDoorSave : ElintriaScript
{
    public void OnTriggered() { SaveSystem.Save(GetComponent<AutoSave>().slot, "Before the boss"); }
}
}
