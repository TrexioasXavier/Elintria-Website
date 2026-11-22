namespace DocCheck_UIRuntime
{
using Elintria;
using System.Globalization;

public class DesignedWidgets : ElintriaScript
{
    public Transform player;
    public float worldSize = 200f;
    public string[] items = { "Images/sword.png", "Images/potion.png" };
    public int[] counts = { 1, 5 };

    public override void Update()
    {
        // The minimap follows the player and turns with them.
        if (player != null)
        {
            UIRuntime.SetProp("Minimap", "mapx", (0.5f + player.position.x / worldSize).ToString(CultureInfo.InvariantCulture));
            UIRuntime.SetProp("Minimap", "mapy", (0.5f - player.position.z / worldSize).ToString(CultureInfo.InvariantCulture));
            UIRuntime.SetProp("Minimap", "maprot", player.eulerAngles.y.ToString(CultureInfo.InvariantCulture));
        }
        // The bag's pictures and counts, slot by slot.
        for (int i = 0; i < items.Length; ++i)
        {
            UIRuntime.SetProp("Bag", "slot" + i, items[i]);
            UIRuntime.SetProp("Bag", "count" + i, counts[i].ToString());
        }
        // Which option the dropdown shows.
        if (UIRuntime.WasClicked("Quality"))
            Debug.Log("Quality option " + UIRuntime.GetProp("Quality", "sel"));
    }
}
}
