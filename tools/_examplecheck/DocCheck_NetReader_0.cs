namespace DocCheck_NetReader
{
using System.Collections.Generic;
using Elintria.Netcode;

public static class ReadAll
{
    public static List<int> Ints(string message)
    {
        var r = new NetReader(message.Split('|'), 0);
        var list = new List<int>();
        while (r.More) list.Add(r.Int());
        return list;
    }
}
}
