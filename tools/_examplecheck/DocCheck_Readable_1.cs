namespace DocCheck_Readable
{
using Elintria;
using Elintria.Standard;

public class NotesFromFile : ElintriaScript
{
    public override void Start()
    {
        string[] lines = Content.ReadAllText("Assets/Story/notes.txt").Split('\n');
        int i = 0;
        foreach (Readable r in FindObjectsOfType<Readable>())
            if (i < lines.Length) r.text = lines[i++];
    }
}
}
