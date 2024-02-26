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

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickAction);

        _towerSelectorScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<TowerSelectorScript>();
    }

    private void ClickAction()
    {
        _towerSelectorScript.ChangeSelection(type);
    }
}
