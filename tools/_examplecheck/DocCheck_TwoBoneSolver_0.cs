namespace DocCheck_TwoBoneSolver
{
using Elintria;
using Elintria.Standard;

public class RobotArm : ElintriaScript
{
    public GameObject shoulder, elbow, hand, goal, pole;

    public override void LateUpdate()
    {
        TwoBoneSolver.Solve(shoulder.transform.position, elbow.transform.position, hand.transform.position,
                            shoulder.transform.rotation, elbow.transform.rotation,
                            goal.transform.position, pole.transform.position,
                            out Quaternion newShoulder, out Quaternion newElbow);
        shoulder.transform.rotation = newShoulder;
        elbow.transform.rotation = newElbow;
    }
}
}
