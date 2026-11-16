namespace DocCheck_SliceMesh
{
using Elintria;
using Elintria.Standard;

public class SwordSlice : ElintriaScript
{
    public string modelPath = "Assets/Models/melon.obj";

    public void Slice(Vector3 swingDirection)
    {
        SliceMesh melon = SliceMesh.FromObj(modelPath);
        Vector3 normal = Vector3.Cross(swingDirection, Vector3.up).normalized;
        melon.Split(melon.Centre(), normal, out SliceMesh left, out SliceMesh right);
        Debug.Log("Pieces: " + left.tri.Count / 3 + " and " + right.tri.Count / 3 + " triangles");
    }
}
}
