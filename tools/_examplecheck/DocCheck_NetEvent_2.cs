namespace DocCheck_NetEvent
{
using Elintria;
using Elintria.Netcode;

public class Traffic : ElintriaScript
{
    public int messages, bytes;

    public void Count(NetEvent e)
    {
        if (e.kind != NetEventKind.Data) return;
        messages++;
        bytes += e.data.Length;
    }
}
}
