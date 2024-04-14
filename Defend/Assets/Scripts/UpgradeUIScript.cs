using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIScript : MonoBehaviour
{
    private GameObject _selectedTower;

    private TMP_Text _normalDamageText;
    private TMP_Text _siegeDamageText;
    private TMP_Text _magicDamageText;

    private Button _normalDamageButton;
    private Button _siegeDamageButton;
    private Button _magicDamageButton;
    
    private TMP_Text _normalDamagePriceText;
    private TMP_Text _siegeDamagePriceText;
    private TMP_Text _magicDamagePriceText;
    
    private float _shakeAmount = 20;
    private float _shakeSpeed = 20;
    private float _shakeTime;

    private Vector3 _defaultPosition = new Vector3();
    private Vector3 _normalDamageButtonDefaultPosition;
    private Vector3 _siegeDamageButtonDefaultPosition;
    private Vector3 _magicDamageButtonDefaultPosition;

    private EconomyManager _economyManager;

    private void Awake()
    {
        _normalDamageText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _siegeDamageText = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _magicDamageText = transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();

        _normalDamageButton = transform.GetChild(1).GetChild(2).GetComponent<Button>();
        _siegeDamageButton = transform.GetChild(2).GetChild(2).GetComponent<Button>();
        _magicDamageButton = transform.GetChild(3).GetChild(2).GetComponent<Button>();
        
        _normalDamagePriceText = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        _siegeDamagePriceText = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        _magicDamagePriceText = transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        
        _normalDamageButton.onClick.AddListener(UpgradeNormal);
        _siegeDamageButton.onClick.AddListener(UpgradeSiege);
        _magicDamageButton.onClick.AddListener(UpgradeMagic);

        _defaultPosition = transform.position;
        _normalDamageButtonDefaultPosition = _normalDamageButton.transform.position;
        _siegeDamageButtonDefaultPosition = _siegeDamageButton.transform.position;
        _magicDamageButtonDefaultPosition = _magicDamageButton.transform.position;

        _economyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EconomyManager>();
    }

    private void Start()
    {
        CloseUpgrade();
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            float offsetX = Mathf.Sin(_shakeTime * _shakeSpeed) * _shakeAmount;
            transform.position = _defaultPosition + new Vector3(offsetX, 0, 0);
            _shakeTime -= 1 * Time.deltaTime;
        }
    }
    
    public void Shake()
    {
        _shakeTime = 0.5f;
    }

    private void UpgradeNormal()
    {
        BaseTowerScript towerScript = _selectedTower.GetComponent<BaseTowerScript>();
        
        if (_economyManager.CanBuyUpgrade(towerScript.normalDamageUpgradePrice))
        {
            towerScript.normalDamage += towerScript.upgradeDamageIncrement;
            _economyManager.ReduceMoney(towerScript.normalDamageUpgradePrice);
            towerScript.normalDamageUpgradePrice += towerScript.upgradePriceIncrement;
            ChangeValues();
            UnSuggestUpgrades();
        }
        else
        {
            Shake();
        }
    }
    
    private void UpgradeSiege()
    {
        BaseTowerScript towerScript = _selectedTower.GetComponent<BaseTowerScript>();
        
        if (_economyManager.CanBuyUpgrade(towerScript.siegeDamageUpgradePrice))
        {
            towerScript.siegeDamage += towerScript.upgradeDamageIncrement;
            _economyManager.ReduceMoney(towerScript.siegeDamageUpgradePrice);
            towerScript.siegeDamageUpgradePrice += towerScript.upgradePriceIncrement;
            ChangeValues();
            UnSuggestUpgrades();
        }
        else
        {
            Shake();
        }
    }
    
    private void UpgradeMagic()
    {
        BaseTowerScript towerScript = _selectedTower.GetComponent<BaseTowerScript>();
        
        if (_economyManager.CanBuyUpgrade(towerScript.magicDamageUpgradePrice))
        {
            towerScript.magicDamage += towerScript.upgradeDamageIncrement;
            _economyManager.ReduceMoney(towerScript.magicDamageUpgradePrice);
            towerScript.magicDamageUpgradePrice += towerScript.upgradePriceIncrement;
            ChangeValues();
            UnSuggestUpgrades();
        }
        else
        {
            Shake();
        }
    }

    private void ChangeValues()
    {
        _normalDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().normalDamage.ToString();
        _siegeDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().siegeDamage.ToString();
        _magicDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().magicDamage.ToString();
        
        BaseTowerScript towerScript = _selectedTower.GetComponent<BaseTowerScript>();
        _normalDamagePriceText.text = "$" + towerScript.normalDamageUpgradePrice;
        _siegeDamagePriceText.text = "$" + towerScript.siegeDamageUpgradePrice;
        _magicDamagePriceText.text = "$" + towerScript.magicDamageUpgradePrice;
    }

    public void OpenUpgrade(GameObject tower)
    {
        _selectedTower = tower;
        
        ChangeValues();
        gameObject.SetActive(true);
    }

    public void CloseUpgrade()
    {
        gameObject.SetActive(false);
        UnSuggestUpgrades();
    }

    public void SuggestByType(TowerManagerScript.TowerType type)
    {
        switch (type)
        {
            case TowerManagerScript.TowerType.Archer:
                _normalDamageButton.transform.position = _normalDamageButtonDefaultPosition + new Vector3(0, 20, 0);
                return;
            case TowerManagerScript.TowerType.Siege:
                _siegeDamageButton.transform.position = _siegeDamageButtonDefaultPosition + new Vector3(0, 20, 0);
                return;
            case TowerManagerScript.TowerType.Magic:
                _magicDamageButton.transform.position = _magicDamageButtonDefaultPosition + new Vector3(0, 20, 0);
                return;
        }
        
        Debug.Log("Tower type not found");
    }

    public void UnSuggestUpgrades()
    {
        _normalDamageButton.transform.position = _normalDamageButtonDefaultPosition;
        _siegeDamageButton.transform.position = _siegeDamageButtonDefaultPosition;
        _magicDamageButton.transform.position = _magicDamageButtonDefaultPosition;
    }
}
