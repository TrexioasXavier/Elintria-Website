namespace DocCheck_ScriptCamera
{
using Elintria;

public class ShakeCam : ElintriaScript
{
    public float trauma;

    public override void LateUpdate()
    {
        trauma = Mathf.Max(0f, trauma - Time.deltaTime);
        Vector3 shake = Random.insideUnitSphere * trauma * 0.3f;
        ScriptCamera.Set(transform.position + shake, -transform.forward, Vector3.up, 60f, 0.1f, 400f);
    }

    public void AddTrauma(float amount) { trauma = Mathf.Min(1f, trauma + amount); }
}
}
