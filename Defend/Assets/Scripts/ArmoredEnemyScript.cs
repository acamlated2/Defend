using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Armored;
    }
}
