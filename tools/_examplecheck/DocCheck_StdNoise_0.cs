namespace DocCheck_StdNoise
{
using Elintria;
using Elintria.Standard;

public class LoudBang : ElintriaScript
{
    public void OnDeath() { StdNoise.Make(transform.position, 60f, gameObject); }
}
}
