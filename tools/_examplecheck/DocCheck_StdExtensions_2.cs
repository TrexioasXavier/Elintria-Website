namespace DocCheck_StdExtensions
{
using Elintria;
using Elintria.Standard;

public class GroupCounter : ElintriaScript
{
    public override void Start()
    {
        int n = 0;
        foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>())
            if (g != gameObject && g.transform.IsChildOf(transform)) ++n;
        Debug.Log(name + " holds " + n + " objects");
    }
}
}
