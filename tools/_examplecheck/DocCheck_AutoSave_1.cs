namespace DocCheck_AutoSave
{
using Elintria;
using Elintria.Standard;

public class Hardcore : ElintriaScript
{
    public bool hardcore;

    public override void Start() { GetComponent<AutoSave>().quickKeys = !hardcore; }
}
}
