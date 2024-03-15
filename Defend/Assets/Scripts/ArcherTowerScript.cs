using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Archer;

        normalDamage = 2;
        siegeDamage = 0.5f;
        magicDamage = 0.5f;
    }
}
