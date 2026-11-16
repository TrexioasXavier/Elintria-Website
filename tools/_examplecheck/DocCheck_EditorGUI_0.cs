namespace DocCheck_EditorGUI
{
using Elintria;

public class PaletteWindow : EditorWindow
{
    int chosen;
    Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.gray };

    [MenuItem("Window/My Tools/Palette")]
    static void Open() { GetWindow<PaletteWindow>("Palette"); }

    void OnGUI()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            Rect r = new Rect(10 + (i % 4) * 44, 10 + (i / 4) * 44, 40, 40);
            EditorGUI.DrawRect(r, colors[i]);
            if (i == chosen) EditorGUI.DrawOutline(r, Color.white);
            if (EditorGUI.Button(new Rect(r.x, r.y + 30, 40, 10), "")) chosen = i;
            EditorGUI.Tooltip(r, colors[i].ToString());
        }
        EditorGUI.Label(new Rect(10, 104, 200, EditorGUI.LineHeight), "Chosen: " + colors[chosen], EditorGUI.Mono, false);
    }
}
}
