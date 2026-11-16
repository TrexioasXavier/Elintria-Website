namespace DocCheck_Readable
{
using Elintria;
using Elintria.Standard;

public class NoteQuest : ElintriaScript
{
    public Readable note;
    public LevelObjective objective;
    private bool m_started;

    public override void Update()
    {
        if (m_started || !note.read) return;
        m_started = true;
        objective.enabled = true;
    }
}
}
