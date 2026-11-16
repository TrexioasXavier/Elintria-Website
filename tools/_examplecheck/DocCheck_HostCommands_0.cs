namespace DocCheck_HostCommands
{
using Elintria;

public class Cheats : ElintriaScript
{
    public override void Update()
    {
        string cmd;
        while ((cmd = HostCommands.Take("cheat.")) != null)
        {
            if (cmd == "cheat.god") { SendMessage("SetInvulnerable", true); HostCommands.Reply("god mode on"); }
            else if (cmd.StartsWith("cheat.give ")) { SendMessage("Give", cmd.Substring(11)); HostCommands.Reply("gave " + cmd.Substring(11)); }
        }
    }
}
}
