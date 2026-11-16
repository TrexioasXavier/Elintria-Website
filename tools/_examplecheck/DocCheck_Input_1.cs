namespace DocCheck_Input
{
using Elintria;

public class Hotkeys : ElintriaScript
{
    public GameObject map;
    float charge;

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && map != null) map.SetActive(!map.activeSelf);

        if (Input.GetKey(KeyCode.Control) && Input.GetKeyDown(KeyCode.S)) Debug.Log("Quick save");

        if (Input.GetKey(KeyCode.F)) charge += Time.deltaTime;          // held
        if (Input.GetKeyUp(KeyCode.F)) { Debug.Log("Threw with power " + charge); charge = 0f; }

        if (Input.GetKey("left shift")) Debug.Log("Sprinting");          // by Unity key name
    }
}
}
