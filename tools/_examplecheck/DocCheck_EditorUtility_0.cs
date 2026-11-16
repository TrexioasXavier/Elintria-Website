namespace DocCheck_EditorUtility
{
using Elintria;

public static class ClearScene
{
    [MenuItem("Tools/Delete All Enemies")]
    static void DeleteEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (!EditorUtility.DisplayDialog("Delete enemies", "Delete " + enemies.Length + " enemies?", "Delete", "Cancel")) return;
        foreach (GameObject e in enemies) GameObject.Destroy(e);
    }
}
}
