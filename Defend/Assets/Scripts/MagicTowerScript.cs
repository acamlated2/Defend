using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Magic;
    }
}
