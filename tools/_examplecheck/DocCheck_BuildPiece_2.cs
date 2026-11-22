namespace DocCheck_BuildPiece
{
using Elintria;
using Elintria.Standard;

public class PieceHealth : ElintriaScript
{
    public void OnDeath()
    {
        GameObject p = GameObject.FindWithTag("Player");
        BuildingPlacer b = p != null ? p.GetComponent<BuildingPlacer>() : null;
        if (b != null) b.placed.Remove(gameObject);   // gone, no refund
    }
}
}
