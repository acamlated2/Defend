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

    private void Awake()
    {
        _normalDamageText = transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _siegeDamageText = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _magicDamageText = transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();

        _normalDamageButton = transform.GetChild(1).GetChild(2).GetComponent<Button>();
        _siegeDamageButton = transform.GetChild(2).GetChild(2).GetComponent<Button>();
        _magicDamageButton = transform.GetChild(3).GetChild(2).GetComponent<Button>();
        
        _normalDamageButton.onClick.AddListener(UpgradeNormal);
        _siegeDamageButton.onClick.AddListener(UpgradeSiege);
        _magicDamageButton.onClick.AddListener(UpgradeMagic);
    }

    private void Start()
    {
        CloseUpgrade();
    }

    private void UpgradeNormal()
    {
        _selectedTower.GetComponent<BaseTowerScript>().normalDamage += 1;
        ChangeValues();
    }
    
    private void UpgradeSiege()
    {
        _selectedTower.GetComponent<BaseTowerScript>().siegeDamage += 1;
        ChangeValues();
    }
    
    private void UpgradeMagic()
    {
        _selectedTower.GetComponent<BaseTowerScript>().magicDamage += 1;
        ChangeValues();
    }

    private void ChangeValues()
    {
        _normalDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().normalDamage.ToString();
        _siegeDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().siegeDamage.ToString();
        _magicDamageText.text = _selectedTower.GetComponent<BaseTowerScript>().magicDamage.ToString();
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
    }
}
