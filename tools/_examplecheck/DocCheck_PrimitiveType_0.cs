namespace DocCheck_PrimitiveType
{
using System;
using Elintria;

public class ShapeShowcase : ElintriaScript
{
    public override void Start()
    {
        PrimitiveType[] all = (PrimitiveType[])Enum.GetValues(typeof(PrimitiveType));
        for (int i = 0; i < all.Length; i++)
        {
            GameObject g = GameObject.CreatePrimitive(all[i]);
            g.name = all[i].ToString();
            g.transform.position = new Vector3(i * 2.5f, 1f, 0f);
        }
    }
}
}
