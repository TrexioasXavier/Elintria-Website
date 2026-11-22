namespace DocCheck_StdThrowKinds
{
using Elintria;
using Elintria.Standard;

public class ThrowableList : ElintriaScript
{
    public override void Update()
    {
        ThrowableUser t = GetComponent<ThrowableUser>();
        float y = 140f;
        for (int k = 0; k < StdThrowKinds.names.Length; ++k, y += 18f)
            ScreenGUI.Text(20f, y, StdThrowKinds.names[k] + "  " + t.Count(k), null, Color.white);
    }
}
}
