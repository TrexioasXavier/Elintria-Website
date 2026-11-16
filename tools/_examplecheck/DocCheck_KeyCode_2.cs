namespace DocCheck_KeyCode
{
using Elintria;

public class ArrowMenu : ElintriaScript
{
    string[] items = { "Continue", "Options", "Quit" };
    int index;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) index = (index + 1) % items.Length;
        if (Input.GetKeyDown(KeyCode.UpArrow)) index = (index + items.Length - 1) % items.Length;
        if (Input.GetKeyDown(KeyCode.Return)) Debug.Log("Chose " + items[index]);

        for (int i = 0; i < items.Length; i++)
            ScreenGUI.Text(60, 100 + i * 30, (i == index ? "> " : "  ") + items[i], null, i == index ? Color.yellow : Color.white);
    }
}
}
