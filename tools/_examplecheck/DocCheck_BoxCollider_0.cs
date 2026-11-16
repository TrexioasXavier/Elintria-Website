namespace DocCheck_BoxCollider
{
using Elintria;

public class RoomVolume : ElintriaScript
{
    public Vector3 roomSize = new Vector3(8f, 3f, 6f);

    public override void Start()
    {
        BoxCollider box = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
        box.size = roomSize;
        box.center = new Vector3(0f, roomSize.y * 0.5f, 0f);
        box.isTrigger = true;
    }

    public override void OnTriggerEnter(Collider other) { Debug.Log(other.gameObject.name + " entered " + name); }
}
}
