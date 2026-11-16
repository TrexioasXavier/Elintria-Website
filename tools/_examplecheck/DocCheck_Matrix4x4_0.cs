namespace DocCheck_Matrix4x4
{
using System.Collections.Generic;
using Elintria;

public class Formation : ElintriaScript
{
    public List<GameObject> followers = new List<GameObject>();
    public float spacing = 2f;

    public override void Update()
    {
        Matrix4x4 m = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        for (int i = 0; i < followers.Count; i++)
        {
            int row = i / 3 + 1, col = i % 3 - 1;
            Vector3 slot = new Vector3(col * spacing, 0f, -row * spacing);   // behind the leader
            followers[i].transform.position = m.MultiplyPoint3x4(slot);
            followers[i].transform.rotation = transform.rotation;
        }
    }
}
}
