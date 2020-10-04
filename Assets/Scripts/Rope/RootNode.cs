using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : StaticNode
{
    public float RopeLength;

    public override void Start()
    {
        base.Start();
        rg = child as RopeGod;
        AllotRopeLength(RopeLength);
    }
}
