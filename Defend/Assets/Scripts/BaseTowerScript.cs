using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BaseTowerScript : MonoBehaviour
{
    public TowerManagerScript.TowerType type = TowerManagerScript.TowerType.Archer;
    
    public float normalDamage = 2;
    public float siegeDamage = 2;
    public float magicDamage = 2;

    public float normalDamageUpgradePrice = 10;
    public float siegeDamageUpgradePrice = 10;
    public float magicDamageUpgradePrice = 10;

    public float upgradeDamageIncrement = 1;
    public float upgradePriceIncrement = 10;
    
    public float range = 10;
    [SerializeField] protected float defaultRange = 10;
    [SerializeField] protected float attackDelay = 1;
    private float _attackTimer;

    public HashSet<GameObject> enemiesInRange = new HashSet<GameObject>();

    private float _mostAdvancedEnemyDistance = -1;
    public GameObject target;

    private GameObject _gameController;

    private void OnDisable()
    {
        target = null;
    }

    protected virtual void Awake()
    {
        range = defaultRange;
        
        GetComponentInChildren<SphereCollider>().radius = range / 2;
        _attackTimer = attackDelay;
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        
        _gameController.GetComponent<TowerManagerScript>().AddToSet(gameObject);
    }

    private void Update()
    {
        if (target == null ||
            enemiesInRange.Count > 0)
        {
            target = GetMostAdvancedEnemy();
        }
        
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.yellow);
            
            _attackTimer -= 1 * Time.deltaTime;
            if (_attackTimer <= 0)
            {
                _attackTimer = attackDelay;
                
                Attack();
            }
        }
    }
    
    protected void Attack()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        GameObject newProjectile = _gameController.transform.GetChild(0).GetComponent<ObjectPoolScript>().GetObject();
        newProjectile.transform.position = transform.position;
        newProjectile.transform.rotation = Quaternion.LookRotation(dir);
        newProjectile.GetComponent<ProjectileScript>().normalDamage = normalDamage;
        newProjectile.GetComponent<ProjectileScript>().siegeDamage = siegeDamage;
        newProjectile.GetComponent<ProjectileScript>().magicDamage = magicDamage;
        newProjectile.GetComponent<ProjectileScript>().owner = transform.gameObject;
        newProjectile.GetComponent<ProjectileScript>().target = target;
    }

    public GameObject GetMostAdvancedEnemy()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = null;
            float shortestDistance = float.PositiveInfinity;

            foreach (var enemy in enemiesInRange)
            {
                BaseEnemyScript enemyScript = enemy.GetComponent<BaseEnemyScript>();
                
                enemyScript.GetAgentRemainingDistance();
                float distance = enemyScript.distanceToBase;

                if (distance < shortestDistance)
                {
                    closestEnemy = enemy;
                    shortestDistance = distance;
                }
            }

            return closestEnemy;
        }

        target = null;
        return null;
    }

    public void RemoveEnemyFromSet(GameObject enemyToRemove)
    {
        if (enemiesInRange.Contains(enemyToRemove))
        {
            enemiesInRange.Remove(enemyToRemove);
            if (enemyToRemove == target)
            {
                GetMostAdvancedEnemy();
            }
        }
    }

    public void ChangeRange(int height)
    {
        range = CalculateRange(height);
        GetComponentInChildren<SphereCollider>().radius = range / 2;
    }

    public float CalculateRange(int height)
    {
        return 10 + 3 * height;
    }

    public float GetTypeSpecificDamageByType(TowerManagerScript.TowerType towerType)
    {
        switch (towerType)
        {
            case TowerManagerScript.TowerType.Archer:
                return normalDamage;
            case TowerManagerScript.TowerType.Siege:
                return siegeDamage;
            case TowerManagerScript.TowerType.Magic:
                return magicDamage;
        }
        
        Debug.Log("Tower type not found");
        return normalDamage;
    }

    public float GetTypeSpecificUpgradePriceByType(TowerManagerScript.TowerType towerType)
    {
        switch (towerType)
        {
            case TowerManagerScript.TowerType.Archer:
                return normalDamageUpgradePrice;
            case TowerManagerScript.TowerType.Siege:
                return siegeDamageUpgradePrice;
            case TowerManagerScript.TowerType.Magic:
                return magicDamageUpgradePrice;
        }
        
        Debug.Log("Tower type not found");
        return normalDamage;
    }
}
