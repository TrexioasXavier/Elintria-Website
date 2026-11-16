namespace DocCheck_Elevator
{
using Elintria;
using Elintria.Standard;

public class FloorDisplay : ElintriaScript
{
    public GameObject elevator;

    public void Arrived(int floor) { gameObject.audioSource.PlayOneShot("Audio/ding.wav"); }   // set as the onArrive message

    public override void Update()
    {
        Elevator e = elevator.GetComponent<Elevator>();
        ScreenGUI.Text(20, 20, "Floor " + e.floor + (e.moving ? " -> " + e.targetFloor : ""));
    }
}
}
