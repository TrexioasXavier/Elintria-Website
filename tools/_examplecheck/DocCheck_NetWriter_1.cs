namespace DocCheck_NetWriter
{
using Elintria;
using Elintria.Netcode;

public class StructMessage : ElintriaScript
{
    public struct Shot { public Vector3 from; public Vector3 dir; public float damage; }

    public static string Encode(Shot s) => new NetWriter().Value(s).ToString();
}
}
