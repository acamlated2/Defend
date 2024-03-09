using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerScript : BaseTowerScript
{
    protected override void Awake()
    {
        base.Awake();
        type = TowerManagerScript.TowerType.Archer;

        normalDamage = 1;
        siegeDamage = 1;
        magicDamage = 1;
    }
}
