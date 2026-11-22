namespace DocCheck_BuildPiece
{
using Elintria;
using Elintria.Standard;

public class PieceValue : ElintriaScript
{
    public override void Update()
    {
        int value = 0;
        foreach (BuildPiece p in FindObjectsOfType<BuildPiece>()) if (p.placed) value += p.Name == "Wall" ? 3 : 1;
        ScreenGUI.Text(20f, 200f, "Base value " + value, null, Color.white);
    }
}
}
