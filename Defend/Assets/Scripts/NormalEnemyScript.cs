using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Normal;
    }
}
