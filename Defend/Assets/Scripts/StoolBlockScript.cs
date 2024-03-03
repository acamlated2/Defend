using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoolBlockScript : BlockScript
{
    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Stool;
    }
}
