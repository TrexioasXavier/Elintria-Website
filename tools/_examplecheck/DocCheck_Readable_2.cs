namespace DocCheck_Readable
{
using Elintria;
using Elintria.Standard;

public class WelcomeSign : ElintriaScript
{
    public override void Start()
    {
        Readable r = gameObject.AddComponent<Readable>();
        r.title = "Welcome";
        r.text = "Find the three fuses and get the lift running.";
        r.prompt = "Read the sign";
    }
}
}
