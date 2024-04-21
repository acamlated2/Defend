using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlockScript : BlockScript
{
    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Platform;
    }
}
