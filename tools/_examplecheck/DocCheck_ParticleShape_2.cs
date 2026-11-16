namespace DocCheck_ParticleShape
{
using Elintria;

public class ShapePicker : ElintriaScript
{
    public int shapeIndex = 2;   // 0 Sphere .. 6 Point

    public override void Start()
    {
        GetComponent<ParticleEmitter>().shape = (ParticleShape)Mathf.Clamp(shapeIndex, 0, 6);
    }
}
}
