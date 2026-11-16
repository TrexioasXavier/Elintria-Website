namespace DocCheck_LevelCheckpoint
{
using Elintria;
using Elintria.Standard;

public class SaveAtCheckpoints : ElintriaScript
{
    private LevelCheckpoint m_last;

    public override void Update()
    {
        if (LevelCheckpoint.current == null || LevelCheckpoint.current == m_last) return;
        m_last = LevelCheckpoint.current;
        Vector3 p = m_last.transform.position;
        System.IO.File.WriteAllText("checkpoint.txt", p.x + " " + p.y + " " + p.z);
    }
}
}
