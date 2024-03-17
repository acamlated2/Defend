using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyScript : MonoBehaviour
{
    public enum Type
    {
        Normal, 
        Shielded, 
        Armored
    }

    public Type type = Type.Normal;
    
    [SerializeField] public float health = 5;
    private protected float MaxHealth;
    [SerializeField] public float shield = 5;
    private protected float MaxShield;
    [SerializeField] public float armor = 5;
    private protected float MaxArmor;
    
    public float damage = 1;

    private GameObject _gameController;

    public GameObject statusBar;

    private void OnEnable()
    {
        health = MaxHealth;
        shield = MaxShield;
        armor = MaxArmor;
    }

    protected virtual void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Damage(float normalDamage, float siegeDamage, float magicDamage)
    {
        armor -= magicDamage;
        statusBar.GetComponent<StatusBarScript>().ChangeArmor(armor);
        if (armor > 0)
        {
            return;
        }
        
        shield -= siegeDamage;
        statusBar.GetComponent<StatusBarScript>().ChangeShield(shield);
        if (shield > 0)
        {
            return;
        }
        
        health -= normalDamage;
        statusBar.GetComponent<StatusBarScript>().ChangeHealth(health);
        if (health > 0)
        {
            return;
        }

        if (health <= 0)
        {
            _gameController.GetComponent<EnemySpawningScript>().ReturnEnemy(gameObject);
            
            _gameController.GetComponent<TowerManagerScript>().RemoveEnemyFromTower(gameObject);
        }
    }
}
