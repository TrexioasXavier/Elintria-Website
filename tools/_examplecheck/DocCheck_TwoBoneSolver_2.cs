namespace DocCheck_TwoBoneSolver
{
using Elintria;
using Elintria.Standard;

public class SolveDebug : ElintriaScript
{
    public override void Update()
    {
        Vector3 a = Vector3.zero, b = new Vector3(0f, -1f, 0f), c = new Vector3(0f, -2f, 0f);
        TwoBoneSolver.Solve(a, b, c, Quaternion.identity, Quaternion.identity, new Vector3(1f, -1.2f, 0f), new Vector3(0f, -1f, 1f), out Quaternion na, out Quaternion nb);
        Vector3 nb_pos = a + na * (b - a);
        Debug.DrawLine(a, nb_pos, Color.yellow);
        Debug.DrawLine(nb_pos, nb_pos + nb * (c - b), Color.cyan);
    }
}
}
