namespace DocCheck_LSystem
{
using System.Collections.Generic;
using Elintria;
using Elintria.Procedural;

public class Snowflake : ElintriaScript
{
    public override void Start()
    {
        string s = LSystem.Expand("F--F--F", new Dictionary<char, string> { { 'F', "F+F--F+F" } }, 3);
        var lines = LSystem.Turtle(s, 0.2f, 60f, 0f);
        Debug.Log("Snowflake with " + lines.Count + " segments");
    }
}
}
