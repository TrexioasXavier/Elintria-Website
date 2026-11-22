namespace DocCheck_SaveSlotInfo
{
using Elintria;
using Elintria.Standard;

public class PlayTime : ElintriaScript
{
    public int slot = 1;

    public override void Start()
    {
        SaveSlotInfo s = SaveSystem.Info(slot);
        if (!s.exists) return;
        int minutes = (int)(s.playSeconds / 60f);
        Debug.Log("Slot " + s.slot + ": " + minutes / 60 + " h " + minutes % 60 + " min played");
    }
}
}
