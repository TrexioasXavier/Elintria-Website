namespace DocCheck_Quality
{
using Elintria;

public class GraphicsMenu : ElintriaScript
{
    public override void Update()
    {
        string[] names = Quality.names;
        for (int i = 0; i < names.Length; i++)
            if (ScreenGUI.Button(new Rect(40, 60 + i * 44, 200, 38), (i == Quality.level ? "> " : "") + names[i]))
                Quality.SetLevel(i);
    }
}
}
