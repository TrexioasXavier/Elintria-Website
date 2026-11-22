namespace DocCheck_SaveSlotInfo
{
using Elintria;
using Elintria.Standard;

public class SlotList : ElintriaScript
{
    public override void Start()
    {
        for (int i = 1; i <= 3; ++i)
        {
            SaveSlotInfo s = SaveSystem.Info(i);
            string text = s.exists ? s.label + "  " + s.time.ToString("g") : "Empty";
            UIRuntime.SetText("slot" + i + "Text", text);
        }
    }
}
}
