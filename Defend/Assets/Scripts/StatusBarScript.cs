using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class StatusBarScript : MonoBehaviour
{
    private GameObject _healthFill;
    private GameObject _shieldFill;
    private GameObject _armorFill;

    private float _healthValue;
    private float _healthMax;
    private float _displayedHealth;
    private float _shieldValue;
    private float _shieldMax;
    private float _displayedShield;
    private float _armorValue;
    private float _armorMax;
    private float _displayedArmor;

    public GameObject owner;

    private void Awake()
    {
        _healthFill = transform.GetChild(0).gameObject;
        _shieldFill = transform.GetChild(1).gameObject;
        _armorFill = transform.GetChild(2).gameObject;
    }

    public void SetUpBar(float health, float shield, float armor)
    {
        _healthValue = health;
        _healthMax = health;
        _displayedHealth = health;
        _shieldValue = shield;
        _shieldMax = shield;
        _displayedShield = shield;
        _armorValue = armor;
        _armorMax = armor;
        _displayedArmor = armor;
        _healthFill.GetComponent<Image>().fillAmount = 1;
        _shieldFill.GetComponent<Image>().fillAmount = 1;
        _armorFill.GetComponent<Image>().fillAmount = 1;
        
        float barWidth = GetComponent<RectTransform>().rect.width;
        float statsTotal = health + shield + armor;

        float healthWidth = (health / statsTotal) * barWidth;
        float shieldWidth = (shield / statsTotal) * barWidth;
        float armorWidth = (armor / statsTotal) * barWidth;

        _healthFill.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(barWidth / 2) + healthWidth / 2, 0);
        _healthFill.GetComponent<RectTransform>().sizeDelta = new Vector2(healthWidth, 10);
        
        _shieldFill.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(_healthFill.transform.GetComponent<RectTransform>().localPosition.x + healthWidth / 2 + shieldWidth / 2, 0);
        _shieldFill.GetComponent<RectTransform>().sizeDelta = new Vector2(shieldWidth, 10);

        _armorFill.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            _shieldFill.transform.GetComponent<RectTransform>().localPosition.x + shieldWidth / 2 + armorWidth / 2, 0);
        _armorFill.GetComponent<RectTransform>().sizeDelta = new Vector2(armorWidth, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            _healthValue -= 1;
        }
        if (Input.GetKeyDown("w"))
        {
            _shieldValue -= 1;
        }
        if (Input.GetKeyDown("e"))
        {
            _armorValue -= 1;
        }
        
        if (_displayedHealth != _healthValue)
        {
            _displayedHealth = Mathf.Lerp(_displayedHealth, _healthValue, 0.01f);
            _healthFill.GetComponent<Image>().fillAmount = _displayedHealth / _healthMax;
        }
        
        if (_displayedShield != _shieldValue)
        {
            _displayedShield = Mathf.Lerp(_displayedShield, _shieldValue, 0.01f);
            _shieldFill.GetComponent<Image>().fillAmount = _displayedShield / _shieldMax;
        }
        
        if (_displayedArmor != _armorValue)
        {
            _displayedArmor = Mathf.Lerp(_displayedArmor, _armorValue, 0.01f);
            _armorFill.GetComponent<Image>().fillAmount = _displayedArmor / _armorMax;
        }

        if (owner)
        {
            transform.position = Camera.main.WorldToScreenPoint(owner.transform.position) + new Vector3(0, 2, 0);
        }
    }

    public void ChangeHealth(float newHealth)
    {
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        
        _healthValue = newHealth;
    }
    
    public void ChangeShield(float newShield)
    {
        if (newShield < 0)
        {
            newShield = 0;
        }
        
        _shieldValue = newShield;
    }
    
    public void ChangeArmor(float newArmor)
    {
        if (newArmor < 0)
        {
            newArmor = 0;
        }
        
        _armorValue = newArmor;
    }
}
