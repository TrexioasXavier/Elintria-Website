namespace DocCheck_SaveSlotInfo
{
using Elintria;
using Elintria.Standard;

public class ContinueButton : ElintriaScript
{
    public override void Update()
    {
        if (!UIRuntime.WasClicked("ContinueButton")) return;
        SaveSlotInfo newest = null;
        for (int i = 1; i <= 5; ++i)
        {
            SaveSlotInfo s = SaveSystem.Info(i);
            if (s.exists && (newest == null || s.time > newest.time)) newest = s;
        }
        if (newest != null) SaveSystem.Load(newest.slot);
    }
}
}
