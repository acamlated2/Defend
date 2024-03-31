using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagerScript : MonoBehaviour
{
    public enum TowerType
    {
        Archer, 
        Siege, 
        Magic
    }
    
    public GameObject[] towerPrefabs;
    
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

    public bool CreateNewTower(Vector3 position, int height)
    {
        TowerType type = GetComponent<TowerSelectorScript>().GetSelectedType();
        
        if (!GetComponent<EconomyManager>().CanBuyTower(type))
        {
            return false;
        }
        
        GetComponent<EconomyManager>().BuyTower(type);

        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            if (towerPrefabs[i].GetComponent<BaseTowerScript>().type == type)
            {
                GameObject newTower = Instantiate(towerPrefabs[i], position, Quaternion.identity);
                AddToSet(newTower);
                
                newTower.GetComponent<BaseTowerScript>().ChangeRange(height);
                
                GetComponent<AIHelperScript>().AssignCoverageValues(newTower);
                
                break;
            }
        }
        return true;
    }
}
