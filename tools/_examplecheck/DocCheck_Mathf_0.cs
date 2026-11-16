namespace DocCheck_Mathf
{
using Elintria;

public class HealthBar : ElintriaScript
{
    public float health = 75f;
    public float maxHealth = 100f;

    public override void Update()
    {
        health = Mathf.Clamp(health, 0f, maxHealth);
        float fill = Mathf.InverseLerp(0f, maxHealth, health);   // 0..1
        Color bar = Color.Lerp(Color.red, Color.green, fill);
        ScreenGUI.Bar(new Rect(20, 20, 200, 18), fill, bar, new Color(0f, 0f, 0f, 0.5f));
        ScreenGUI.Text(230, 20, Mathf.RoundToInt(health) + " / " + Mathf.RoundToInt(maxHealth));
    }
}
}
