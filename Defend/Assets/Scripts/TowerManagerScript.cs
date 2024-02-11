using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagerScript : MonoBehaviour
{
    public HashSet<GameObject> Towers = new HashSet<GameObject>();

    public void AddToSet(GameObject tower)
    {
        Towers.Add(tower);
    }

    public void RemoveTowerFromSet(GameObject tower)
    {
        if (Towers.Contains(tower))
        {
            Towers.Remove(tower);
        }
    }

    public void RemoveEnemyFromTower(GameObject enemy)
    {
        foreach (var tower in Towers)
        {
            tower.GetComponent<BaseTowerScript>().RemoveEnemyFromSet(enemy);
        }
    }
}
