namespace DocCheck_Transform
{
using Elintria;

public class Turntable : ElintriaScript
{
    public float degreesPerSecond = 30f;
    public GameObject newcomer;

    public override void Start()
    {
        Debug.Log(name + " has " + transform.childCount + " children:");
        foreach (Transform child in transform)
            Debug.Log(" - " + child.name + " at local " + child.localPosition);

        Transform lid = transform.Find("Box/Lid");          // a grandchild, by path
        if (lid != null) lid.localEulerAngles = new Vector3(-60f, 0f, 0f);

        if (newcomer != null)
            newcomer.transform.SetParent(transform, true);  // keeps its world position
    }

    public override void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).RotateAround(transform.position, Vector3.up, degreesPerSecond * Time.deltaTime);
    }
}
}
