using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Siege;
    }
}
