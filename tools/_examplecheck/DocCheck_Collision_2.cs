namespace DocCheck_Collision
{
using System.Collections.Generic;
using Elintria;

public class ContactCounter : ElintriaScript
{
    readonly HashSet<GameObject> touching = new HashSet<GameObject>();

    public override void OnCollisionEnter(Collision c) { touching.Add(c.gameObject); }
    public override void OnCollisionExit(Collision c) { touching.Remove(c.gameObject); }

    public override void Update() { ScreenGUI.Text(20, 40, name + " touches " + touching.Count + " objects"); }
}
}
