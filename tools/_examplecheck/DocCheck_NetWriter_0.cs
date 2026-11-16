namespace DocCheck_NetWriter
{
using Elintria;
using Elintria.Netcode;

public class CustomMessage : ElintriaScript
{
    public override void Start()
    {
        string msg = new NetWriter().Put("chat").Put(3).Value(new Vector3(1f, 2f, 3f)).Put("hello there").ToString();
        NetReader r = new NetReader(msg.Split('|'), 0);
        Debug.Log(r.Str() + " from " + r.Int() + " at " + r.Value<Vector3>() + ": " + r.Str());
    }
}
}
