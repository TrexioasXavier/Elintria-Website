namespace DocCheck_SaveSystem
{
using Elintria;
using Elintria.Standard;

public class SaveMenu : ElintriaScript
{
    public bool open;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) open = !open;
        if (!open) return;
        for (int i = 1; i <= 3; ++i)
        {
            SaveSlotInfo info = SaveSystem.Info(i);
            string label = "Slot " + i + (info.exists ? "  " + info.time.ToString("g") : "  empty");
            if (ScreenGUI.Button(new Rect(40f, 40f + i * 40f, 260f, 32f), label)) SaveSystem.Save(i, "Manual");
            if (info.exists && ScreenGUI.Button(new Rect(310f, 40f + i * 40f, 80f, 32f), "Load")) SaveSystem.Load(i);
        }
    }
}
}
