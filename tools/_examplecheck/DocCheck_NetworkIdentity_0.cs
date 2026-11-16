namespace DocCheck_NetworkIdentity
{
using Elintria;

public class MyCharacter : ElintriaScript
{
    public override void Update()
    {
        NetworkIdentity id = GetComponent<NetworkIdentity>();
        if (id != null && !id.isMine) return;
        transform.position += new Vector3(Input.GetAxisHorizontal(), 0f, Input.GetAxisVertical()) * 4f * Time.deltaTime;
    }
}
}
