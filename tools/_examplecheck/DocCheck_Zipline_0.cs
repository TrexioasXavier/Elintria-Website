namespace DocCheck_Zipline
{
using Elintria;
using Elintria.Standard;

public class AutoZip : ElintriaScript
{
    public Zipline line;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !line.riding) line.Attach(other.gameObject);
    }
}
}
