using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Magic;
        
        normalDamage = 0.5f;
        siegeDamage = 0.5f;
        magicDamage = 2;
    }
}
