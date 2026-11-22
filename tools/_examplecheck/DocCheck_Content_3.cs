namespace DocCheck_Content
{
using Elintria;

public class DlcMenu : ElintriaScript
{
    public override void Start()
    {
        Content.BundleMounted += name => Debug.Log("DLC ready: " + name);
        foreach (string dlc in Content.Bundles) Debug.Log("Installed: " + dlc);
        foreach (string why in Content.BundleErrors) Debug.LogWarning("Not loaded: " + why);
    }

    public override void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F5)) return;
        string error;
        if (Content.MountBundle("DLC/Expansion", out error))
            SceneManager.LoadScene("ExpansionHub");
        else
            Debug.LogWarning("Could not load the expansion: " + error);
    }
}
}
