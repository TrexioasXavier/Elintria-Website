namespace DocCheck_SplineFollower
{
using Elintria;
using Elintria.Standard;

public class DollyCutscene : ElintriaScript
{
    public GameObject player;

    public override void Start()
    {
        SplineFollower f = GetComponent<SplineFollower>();
        f.mode = 2;            // once
        f.speed = 3f;
        f.faceForward = true;
        f.onEndTargets.Add(player);
        f.onEndMessage = "SetActive:true";   // the player appears when the shot ends
        f.Play();
    }
}
}
