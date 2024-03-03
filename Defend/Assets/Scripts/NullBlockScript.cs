using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullBlockScript : BlockScript
{
    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Null;
    }
}
