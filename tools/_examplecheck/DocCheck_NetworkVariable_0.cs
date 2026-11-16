namespace DocCheck_NetworkVariable
{
using Elintria;
using Elintria.Netcode;

public class NetHealth : NetworkBehaviour
{
    public NetworkVariable<float> health = new NetworkVariable<float>(100f);

    public override void Start()
    {
        health.OnValueChanged += (oldValue, newValue) =>
        {
            if (newValue < oldValue) Debug.Log(name + " took " + (oldValue - newValue) + " damage");
        };
    }

    public void ApplyDamage(float amount)
    {
        if (IsServer) health.Value = Mathf.Max(0f, health.Value - amount);
    }
}
}
