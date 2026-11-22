namespace DocCheck_TdPath
{
using Elintria;
using Elintria.Standard;

public class PathLength : ElintriaScript
{
    public override void Start()
    {
        TdPath p = GetComponent<TdPath>();
        Debug.Log("The road is " + Mathf.Round(p.length) + " m with " + p.Points().Count + " corners");
    }
}
}
