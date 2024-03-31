using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonScript : MonoBehaviour
{
    private Button _button;

    [SerializeField] private TowerManagerScript.TowerType type;

    private TowerSelectorScript _towerSelectorScript;

    private Image _indicator;

    private Vector3 _defaultPosition;

    private float _shakeAmount = 20;
    private float _shakeSpeed = 20;
    private float _shakeTime;

    private TMP_Text _priceText;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickAction);

        _towerSelectorScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<TowerSelectorScript>();

        _indicator = transform.GetChild(1).GetComponent<Image>();
        
        _defaultPosition = transform.position;
        
        _priceText = transform.GetChild(0).GetComponent<TMP_Text>();
        
        UnSuggest();
        UnSelect();
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

    private void ClickAction()
    {
        _towerSelectorScript.ChangeSelection(type);
    }

    public void Suggest()
    {
        transform.position = _defaultPosition + new Vector3(0, 20, 0);
        
    }
    
    public void UnSuggest()
    {
        transform.position = _defaultPosition;
    }

    public void Select()
    {
        _indicator.color = new Color(1, 1, 1, 1);
    }

    public void UnSelect()
    {
        _indicator.color = new Color(1, 1, 1, 0);
    }

    public void ChangePrice(float newPrice)
    {
        _priceText.text = "$" + newPrice;
    }
}
