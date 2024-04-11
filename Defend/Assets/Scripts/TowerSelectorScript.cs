using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectorScript : MonoBehaviour
{
    private TowerManagerScript.TowerType selectedTowerType = TowerManagerScript.TowerType.Archer;
    
    private GameObject _archerTowerButton;
    private GameObject _siegeTowerButton;
    private GameObject _magicTowerButton;
    private GameObject _stoolTowerButton;

    private void Awake()
    {
        _archerTowerButton = GameObject.FindGameObjectWithTag("Archer Tower Button");
        _siegeTowerButton = GameObject.FindGameObjectWithTag("Siege Tower Button");
        _magicTowerButton = GameObject.FindGameObjectWithTag("Magic Tower Button");
        _stoolTowerButton = GameObject.FindGameObjectWithTag("Stool Tower Button");
    }

    private void Start()
    {
        _archerTowerButton.GetComponent<TowerButtonScript>().Select();
    }

    public void ChangeSelection(TowerManagerScript.TowerType type)
    {
        selectedTowerType = type;

        switch (selectedTowerType)
        {
            case TowerManagerScript.TowerType.Archer:
                _archerTowerButton.GetComponent<TowerButtonScript>().Select();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _stoolTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                break;
            case TowerManagerScript.TowerType.Siege:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _siegeTowerButton.GetComponent<TowerButtonScript>().Select();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _stoolTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                break;
            case TowerManagerScript.TowerType.Magic:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _magicTowerButton.GetComponent<TowerButtonScript>().Select();
                _stoolTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                break;
            case TowerManagerScript.TowerType.Stool:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSelect();
                _stoolTowerButton.GetComponent<TowerButtonScript>().Select();
                break;
        }
    }

    public TowerManagerScript.TowerType GetSelectedType()
    {
        return selectedTowerType;
    }

    public GameObject GetSelectedButton()
    {
        switch (selectedTowerType)
        {
            case TowerManagerScript.TowerType.Archer:
                return _archerTowerButton;
            case TowerManagerScript.TowerType.Siege:
                return _siegeTowerButton;
            case TowerManagerScript.TowerType.Magic:
                return _magicTowerButton;
            case TowerManagerScript.TowerType.Stool:
                return _stoolTowerButton;
        }

        Debug.Log("Can't find tower button");
        return _archerTowerButton;
    }

    public GameObject GetButtonByType(TowerManagerScript.TowerType type)
    {
        switch (type)
        {
            case TowerManagerScript.TowerType.Archer:
                return _archerTowerButton;
            case TowerManagerScript.TowerType.Siege:
                return _siegeTowerButton;
            case TowerManagerScript.TowerType.Magic:
                return _magicTowerButton;
            case TowerManagerScript.TowerType.Stool:
                return _stoolTowerButton;
        }

        Debug.Log("Can't find tower button by type");
        return _archerTowerButton;
    }
}
