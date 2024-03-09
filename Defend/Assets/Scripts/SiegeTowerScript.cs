using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Siege;
        
        normalDamage = 0.5f;
        siegeDamage = 2;
        magicDamage = 0.5f;
    }
}
