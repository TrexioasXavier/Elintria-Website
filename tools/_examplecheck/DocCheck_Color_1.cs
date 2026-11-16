namespace DocCheck_Color
{
using Elintria;

public class FloatingText : ElintriaScript
{
    public string text = "+100";
    float age;

    public override void Update()
    {
        age += Time.deltaTime;
        Color c = Color.yellow;
        c.a = Mathf.Clamp01(1f - age / 1.5f);
        ScreenGUI.Label(new Rect(300, 200 - age * 40f, 200, 30), text, null, c);
        if (age > 1.5f) Destroy(gameObject);
    }
}
}
