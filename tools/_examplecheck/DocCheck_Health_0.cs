namespace DocCheck_Health
{
using Elintria;
using Elintria.Standard;

public class PlayerHealthHud : ElintriaScript
{
    public override void Update()
    {
        Health h = GetComponent<Health>();
        ScreenGUI.Bar(new Rect(20, 20, 220, 18), h.health / h.maxHealth, Color.red, new Color(0f, 0f, 0f, 0.5f));
        if (h.isDead) ScreenGUI.Label(new Rect(0, 200, ScreenGUI.screenWidth, 40), "YOU DIED", null, Color.red, TextAnchor.MiddleCenter);
    }
}
}
