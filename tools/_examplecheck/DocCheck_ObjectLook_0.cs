namespace DocCheck_ObjectLook
{
using Elintria;

public class DamagedLook : ElintriaScript
{
    public string damagedModel = "Assets/Models/car_wrecked.fbx";
    public float health = 100f;

    public void ApplyDamage(float amount)
    {
        health -= amount;
        if (health <= 0f && ObjectLook.ModelPath(gameObject) != damagedModel)
            ObjectLook.SetModel(gameObject, damagedModel);
    }
}
}
