namespace DocCheck_SerializeFieldAttribute
{
using Elintria;

public class Alarm : ElintriaScript
{
    [SerializeField] GameObject siren;   // drag an object here

    public void Trigger()
    {
        if (siren != null) siren.audioSource.Play();
    }
}
}
