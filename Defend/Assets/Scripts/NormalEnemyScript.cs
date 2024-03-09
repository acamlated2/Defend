using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Normal;

        health = health * 1;
        shield = shield * 0;
        armor = armor * 0;

        MaxHealth = health;
        MaxShield = shield;
        MaxArmor = armor;
    }
}
