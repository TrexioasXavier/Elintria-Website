namespace DocCheck_Behaviour
{
using Elintria;

public class SafeCall : ElintriaScript
{
    public static void Ping(ElintriaScript s)
    {
        if (s != null && s.isActiveAndEnabled)
            s.SendMessage("OnPing");
    }
}
}
