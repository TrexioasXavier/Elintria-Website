namespace DocCheck_LSystem
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class FractalPlant : ElintriaScript
{
    List<(Vector2 a, Vector2 b)> segs;

    public override void Start()
    {
        var rules = new Dictionary<char, string> { { 'X', "F+[[X]-X]-F[-FX]+X" }, { 'F', "FF" } };
        string s = LSystem.Expand("X", rules, 5);
        segs = LSystem.Turtle(s, 0.05f, 25f);
    }

    public override void Update()
    {
        foreach (var seg in segs)
            Debug.DrawLine(transform.position + (Vector3)seg.a, transform.position + (Vector3)seg.b, Color.green);
    }
}
}
