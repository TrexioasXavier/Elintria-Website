namespace DocCheck_DialogueNpc
{
using Elintria;
using Elintria.Standard;

public class FileDialogue : ElintriaScript
{
    public override void Start() { GetComponent<DialogueNpc>().scriptFile = "Assets/Dialogue/elder.txt"; }
}
}
