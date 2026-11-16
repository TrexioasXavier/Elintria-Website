namespace DocCheck_KeyCode
{
using Elintria;

public class HotbarKeys : ElintriaScript
{
    public int selected;

    public override void Update()
    {
        for (int i = 0; i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) { selected = i; Debug.Log("Slot " + (i + 1)); }
    }
}
}
