using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Shielded;
    }
}
