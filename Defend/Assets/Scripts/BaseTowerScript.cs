using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BaseTowerScript : MonoBehaviour
{
    public TowerManagerScript.TowerType type = TowerManagerScript.TowerType.Archer;
    
    [SerializeField] protected float damage = 2;
    [SerializeField] protected float range = 7;
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
        GetComponentInChildren<SphereCollider>().radius = range;
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
        newProjectile.GetComponent<ProjectileScript>().damage = damage;
        newProjectile.GetComponent<ProjectileScript>().owner = transform.gameObject;
    }

    public GameObject GetMostAdvancedEnemy()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = null;
            float shortestDistance = float.PositiveInfinity;

            foreach (var enemy in enemiesInRange)
            {
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

                float distance = agent.remainingDistance;

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
}
