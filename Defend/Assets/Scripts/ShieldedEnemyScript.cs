using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Shielded;
        
        health = health * 1;
        shield = shield * 1;
        armor = armor * 0;

        MaxHealth = health;
        MaxShield = shield;
        MaxArmor = armor;
    }
}
