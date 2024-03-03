using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlockScript : BlockScript
{
    public bool hasTower;

    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Ground;
    }
}
