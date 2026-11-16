namespace DocCheck_GameObject
{
using System.Collections.Generic;
using Elintria;

public class LightSwitch : ElintriaScript
{
    public List<GameObject> lamps = new List<GameObject>();
    public bool startOn = true;

    public override void Start()
    {
        Set(startOn);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Set(!(lamps.Count > 0 && lamps[0].activeSelf));
    }

    void Set(bool on)
    {
        foreach (GameObject lamp in lamps)
            if (lamp != null && lamp.IsAlive)
                lamp.SetActive(on);
    }
}
}
