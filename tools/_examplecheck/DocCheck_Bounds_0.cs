namespace DocCheck_Bounds
{
using System.Collections.Generic;
using Elintria;

public class GroupBounds : ElintriaScript
{
    public List<GameObject> members = new List<GameObject>();

    public override void Update()
    {
        if (members.Count == 0) return;
        Bounds b = new Bounds(members[0].transform.position, Vector3.zero);
        foreach (GameObject g in members) b.Encapsulate(g.transform.position);
        b.Expand(1f);
        Debug.DrawLine(b.min, b.max, Color.cyan);
        transform.position = b.center + Vector3.up * (b.extents.y + 5f);   // hover a camera above
    }
}
}
