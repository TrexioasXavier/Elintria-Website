namespace DocCheck_Collider
{
using Elintria;

public class WhatDidIHit : ElintriaScript
{
    public override void OnCollisionEnter(Collision collision)
    {
        Collider c = collision.collider;
        string kind = c.attachedRigidbody != null ? "a moving body" : "static scenery";
        Debug.Log("Hit " + c.gameObject.name + " (" + kind + "), its box is " + c.bounds.size);
    }
}
}
