using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemyScript : BaseEnemyScript
{
    protected override void Awake()
    {
        base.Awake();
        type = Type.Armored;
        
        health = health * 0.7f;
        shield = shield * 0.3f;
        armor = armor * 1;

        MaxHealth = health;
        MaxShield = shield;
        MaxArmor = armor;
    }
}
