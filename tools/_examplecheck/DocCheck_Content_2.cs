namespace DocCheck_Content
{
using Elintria;

public class AsciiLevel : ElintriaScript
{
    public string levelFile = "Assets/Levels/level1.txt";

    public override void Start()
    {
        string[] rows = Content.ReadAllLines(levelFile);
        if (rows == null) return;
        for (int z = 0; z < rows.Length; z++)
            for (int x = 0; x < rows[z].Length; x++)
                if (rows[z][x] == '#')
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(x, 0.5f, -z);
                }
    }
}
}
