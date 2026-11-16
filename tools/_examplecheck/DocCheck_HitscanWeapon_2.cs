namespace DocCheck_HitscanWeapon
{
using Elintria;
using Elintria.Standard;

public class HitMarker : ElintriaScript
{
    int lastHits;
    float showUntil;

    public override void Update()
    {
        HitscanWeapon g = GetComponent<HitscanWeapon>();
        if (g.hits != lastHits) { lastHits = g.hits; showUntil = Time.time + 0.15f; Debug.Log("Hit " + g.lastHitObject.name + " at " + g.lastHitPoint); }
        if (Time.time < showUntil) ScreenGUI.Label(new Rect(0, 0, ScreenGUI.screenWidth, ScreenGUI.screenHeight), "X", null, Color.red, TextAnchor.MiddleCenter);
    }
}
}
