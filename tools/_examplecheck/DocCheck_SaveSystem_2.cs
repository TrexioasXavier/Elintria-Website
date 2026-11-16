namespace DocCheck_SaveSystem
{
using Elintria;
using Elintria.Standard;

public class ContinueButton : ElintriaScript
{
    public override void Update()
    {
        if (!SaveSystem.Exists(0)) return;
        if (ScreenGUI.Button(new Rect(40f, 40f, 200f, 40f), "Continue") && !SaveSystem.Load(0)) Debug.LogWarning(SaveSystem.lastError);
    }
}
}
