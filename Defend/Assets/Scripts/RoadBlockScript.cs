using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockScript : BlockScript
{
    public float coverageValue;

    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Road;
    }
}
