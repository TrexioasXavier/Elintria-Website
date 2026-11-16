namespace DocCheck_EditorGUI
{
using Elintria;

public class QuickForm : EditorWindow
{
    string itemName = "Sword";
    float damage = 10f;
    bool rare;

    [MenuItem("Window/My Tools/Quick Form")]
    static void Open() { GetWindow<QuickForm>("Form"); }

    void OnGUI()
    {
        EditorGUI.Label(new Rect(10, 10, 80, 20), "Name");
        itemName = EditorGUI.TextField(new Rect(90, 10, 160, 20), itemName, "name");
        EditorGUI.Label(new Rect(10, 36, 80, 20), "Damage");
        damage = EditorGUI.Slider(new Rect(90, 36, 160, 20), damage, 0f, 100f, "dmg");
        EditorGUI.Label(new Rect(10, 62, 80, 20), "Rare");
        rare = EditorGUI.Toggle(new Rect(90, 62, 20, 20), rare);
        EditorGUI.Separator(new Rect(10, 86, 240, 8));
        if (EditorGUI.Button(new Rect(10, 98, 240, 24), "Save " + itemName)) Debug.Log(itemName + " " + damage + (rare ? " (rare)" : ""));
    }
}
}
