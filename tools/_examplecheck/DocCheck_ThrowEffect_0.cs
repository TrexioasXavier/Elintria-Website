namespace DocCheck_ThrowEffect
{
using Elintria;
using Elintria.Standard;

public class SmokeMachine : ElintriaScript
{
    public void Puff() { ThrowEffect.Make(1, transform.position, 5f, 12f, 0f, gameObject); }
}
}
