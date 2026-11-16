namespace DocCheck_Brain
{
using Elintria;
using Elintria.ML;

public class BrainReload : ElintriaScript
{
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6)) { Brain.Forget(); Debug.Log("Brains will be read again"); }
    }
}
}
