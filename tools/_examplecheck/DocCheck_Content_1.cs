namespace DocCheck_Content
{
using Elintria;

public class LevelList : ElintriaScript
{
    public override void Start()
    {
        if (!Content.DirectoryExists("Assets/Levels")) return;
        foreach (string f in Content.List("Assets/Levels"))
            if (f.EndsWith(".txt"))
                Debug.Log(f + ": " + Content.ReadAllLines("Assets/Levels/" + f).Length + " rows");
    }
}
}
