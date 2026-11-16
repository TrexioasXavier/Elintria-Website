namespace DocCheck_ComponentBase
{
using Elintria;

public class LightFromCollider : ElintriaScript
{
    public override void OnTriggerEnter(Collider other)
    {
        // other is a Collider (a ComponentBase); ask its object for a Light.
        Light lamp = other.GetComponent<Light>();
        if (lamp != null && other.CompareTag("Lamp"))
            lamp.intensity *= 2f;
    }
}
}
