namespace DocCheck_Wfc
{
using Elintria;
using Elintria.Procedural;

public class LearnedIslands : ElintriaScript
{
    public override void Start()
    {
        int[,] sample =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 1, 2, 1, 0 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
        };   // 0 water, 1 sand, 2 forest
        int[,] world = Wfc.FromExample(sample).Run(40, 40, 7);
        Debug.Log(world != null ? "Generated islands" : "Try another seed");
    }
}
}
