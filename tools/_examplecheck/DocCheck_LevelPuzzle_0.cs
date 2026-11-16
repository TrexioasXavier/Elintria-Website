namespace DocCheck_LevelPuzzle
{
using Elintria;
using Elintria.Standard;

public class VaultAlarm : ElintriaScript
{
    public LevelPuzzle puzzle;
    public Spawner guards;
    private bool m_was;

    public override void Update()
    {
        bool typing = puzzle.typing;
        if (m_was && !typing && !puzzle.solved) guards.StartSpawning();     // walked away without the code
        m_was = typing;
    }
}
}
