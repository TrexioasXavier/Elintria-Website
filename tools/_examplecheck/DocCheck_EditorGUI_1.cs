namespace DocCheck_EditorGUI
{
using Elintria;

public class ChartWindow : EditorWindow
{
    float[] values = { 3f, 7f, 2f, 9f, 5f };

    [MenuItem("Window/My Tools/Chart")]
    static void Open() { GetWindow<ChartWindow>("Chart"); }

    void OnGUI()
    {
        float max = 10f, w = (width - 20f) / values.Length;
        for (int i = 0; i < values.Length; i++)
        {
            float h = values[i] / max * (height - 40f);
            EditorGUI.DrawRect(new Rect(10 + i * w, height - 20 - h, w - 6, h), new Color(0.4f, 0.6f, 1f));
            EditorGUI.Label(new Rect(10 + i * w, height - 18, w - 6, 16), values[i].ToString(), EditorGUI.Small, true, 1);
        }
    }
}
}
