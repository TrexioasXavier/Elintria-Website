namespace DocCheck_Weapon
{
using System.Collections.Generic;
using Elintria;
using Elintria.Standard;

public class WeaponSwitch : ElintriaScript
{
    public List<GameObject> weapons = new List<GameObject>();
    int current;

    public override void Start() { Equip(0); }

    public override void Update()
    {
        for (int i = 0; i < weapons.Count && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) Equip(i);
    }

    void Equip(int i)
    {
        current = i;
        for (int k = 0; k < weapons.Count; k++) weapons[k].SetActive(k == i);
    }
}
}
