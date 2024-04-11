using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private float playerMoney = 50;
    private GameObject _playerMoneyUI;

    [SerializeField] private float archerTowerPrice = 10;
    [SerializeField] private float siegeTowerPrice = 10;
    [SerializeField] private float magicTowerPrice = 10;
    [SerializeField] private float stoolTowerPrice = 50;

    [SerializeField] private float towerPriceIncrement = 10;
    [SerializeField] private float stoolPriceIncrement = 20;

    private void Awake()
    {
        _playerMoneyUI = GameObject.FindGameObjectWithTag("Player Money UI");
    }

    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            AddMoney(100);
        }
    }

    public void ReduceMoney(float amount)
    {
        playerMoney -= amount;
        _playerMoneyUI.GetComponent<PlayerMoneyUIScript>().ChangeValue(playerMoney);
    }
    
    public void AddMoney(float amount)
    {
        playerMoney += amount;
        _playerMoneyUI.GetComponent<PlayerMoneyUIScript>().ChangeValue(playerMoney);
    }

    public void BuyTower(TowerManagerScript.TowerType type)
    {
        TowerSelectorScript towerSelectorScript = GetComponent<TowerSelectorScript>();
        
        switch (type)
        {
            case TowerManagerScript.TowerType.Archer:
                ReduceMoney(archerTowerPrice);
                archerTowerPrice += towerPriceIncrement;
                towerSelectorScript.GetButtonByType(TowerManagerScript.TowerType.Archer)
                                   .GetComponent<TowerButtonScript>().ChangePrice(archerTowerPrice);
                break;
            case TowerManagerScript.TowerType.Siege:
                ReduceMoney(siegeTowerPrice);
                siegeTowerPrice += towerPriceIncrement;
                towerSelectorScript.GetButtonByType(TowerManagerScript.TowerType.Siege)
                                   .GetComponent<TowerButtonScript>().ChangePrice(siegeTowerPrice);
                break;
            case TowerManagerScript.TowerType.Magic:
                ReduceMoney(magicTowerPrice);
                magicTowerPrice += towerPriceIncrement;
                towerSelectorScript.GetButtonByType(TowerManagerScript.TowerType.Magic)
                                   .GetComponent<TowerButtonScript>().ChangePrice(magicTowerPrice);
                break;
            case TowerManagerScript.TowerType.Stool:
                ReduceMoney(stoolTowerPrice);
                stoolTowerPrice += stoolPriceIncrement;
                towerSelectorScript.GetButtonByType(TowerManagerScript.TowerType.Stool)
                                   .GetComponent<TowerButtonScript>().ChangePrice(stoolTowerPrice);
                break;
        }
    }

    public bool CanBuyTower(TowerManagerScript.TowerType type)
    {
        switch (type)
        {
            case TowerManagerScript.TowerType.Archer:
                if (playerMoney >= archerTowerPrice)
                {
                    return true;
                }
                break;
            case TowerManagerScript.TowerType.Siege:
                if (playerMoney >= siegeTowerPrice)
                {
                    return true;
                }
                break;
            case TowerManagerScript.TowerType.Magic:
                if (playerMoney >= magicTowerPrice)
                {
                    return true;
                }
                break;
            case TowerManagerScript.TowerType.Stool:
                if (playerMoney >= stoolTowerPrice)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public bool CanBuyUpgrade(float price)
    {
        if (price <= playerMoney)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetTowerPriceByType(TowerManagerScript.TowerType type)
    {
        switch (type)
        {
            case TowerManagerScript.TowerType.Archer:
                return archerTowerPrice;
            case TowerManagerScript.TowerType.Siege:
                return siegeTowerPrice;
            case TowerManagerScript.TowerType.Magic:
                return magicTowerPrice;
            case TowerManagerScript.TowerType.Stool:
                return stoolTowerPrice;
        }
        
        Debug.Log("Tower type not found");
        return archerTowerPrice;
    }
}
