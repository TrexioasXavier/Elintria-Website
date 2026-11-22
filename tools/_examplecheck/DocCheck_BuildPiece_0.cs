namespace DocCheck_BuildPiece
{
using Elintria;
using Elintria.Standard;

public class DoorPieceSetup : ElintriaScript
{
    public override void Awake()
    {
        BuildPiece p = GetComponent<BuildPiece>();
        p.pieceName = "Door";
        p.cost = "money:25";
        p.size = new Vector3(1f, 2.2f, 0.2f);
    }
}
}
