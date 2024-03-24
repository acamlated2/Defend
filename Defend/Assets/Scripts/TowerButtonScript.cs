using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonScript : MonoBehaviour
{
    private Button _button;

    [SerializeField] private TowerManagerScript.TowerType type;

    private TowerSelectorScript _towerSelectorScript;

    private Image _indicator;

    private Vector3 _defaultPosition;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickAction);

        _towerSelectorScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<TowerSelectorScript>();

        _indicator = transform.GetChild(1).GetComponent<Image>();
        
        _defaultPosition = transform.position;
        UnSuggest();
        
        UnSelect();
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
}
