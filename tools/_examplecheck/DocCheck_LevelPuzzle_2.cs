namespace DocCheck_LevelPuzzle
{
using Elintria;
using Elintria.Standard;

public class PuzzleHint : ElintriaScript
{
    public LevelPuzzle puzzle;
    public float after = 90f;

    public override void Update()
    {
        if (puzzle.solved || Time.time < after) return;
        string hint = puzzle.mode == 2 ? "The code starts with " + puzzle.code.Substring(0, 1) : "Look for a note near the levers";
        ScreenGUI.Text(20f, 60f, hint);
    }
}
}
