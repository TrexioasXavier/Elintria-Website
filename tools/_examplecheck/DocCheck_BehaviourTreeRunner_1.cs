namespace DocCheck_BehaviourTreeRunner
{
using Elintria;
using Elintria.AI;

public class MultiScriptNpc : ElintriaScript
{
    public override void Start()
    {
        BehaviourTreeRunner r = GetComponent<BehaviourTreeRunner>();
        if (r != null && r.agent == "") Debug.LogWarning("Set the runner's agent to 'MerchantBrain' so the tree calls the right script");
    }
}

public class MerchantBrain : ElintriaScript
{
    public bool shopOpen = true;          // a condition can read a field
    void GreetCustomer() { Debug.Log("Welcome!"); }
}
}
