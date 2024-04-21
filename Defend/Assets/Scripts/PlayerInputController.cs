using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputController : MonoBehaviour
{
    private Camera _camera;

    private GameObject _indicatorCube;
    private GameObject _upgradeIndicatorCube;

    private Ray _ray;
    private RaycastHit _hit;

    private GameObject _upgradeUI;

    private void Awake()
    {
        _camera = Camera.main;

        _indicatorCube = GameObject.FindGameObjectWithTag("IndicatorCube");
        _upgradeIndicatorCube = Instantiate(_indicatorCube);
        _upgradeIndicatorCube.transform.name = "Upgrade Indicator Cube";

        _upgradeUI = GameObject.FindGameObjectWithTag("Upgrade UI");
    }

    private void Update()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            HandleTower();
            HandleUpgrade();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                
                _upgradeUI.GetComponent<UpgradeUIScript>().CloseUpgrade();
                HideUpgradeIndicatorCube();
            }
            
            HideIndicatorCube();
        }
    }

    private void HideIndicatorCube()
    {
        if (_indicatorCube.activeInHierarchy)
        {
            _indicatorCube.SetActive(false);
        }
    }

    private void ShowIndicatorCube()
    {
        if (!_indicatorCube.activeInHierarchy)
        {
            _indicatorCube.SetActive(true);
        }
    }

    private void HandleTower()
    {
        if (_hit.transform.CompareTag("Block"))
        {
            if (_hit.transform.GetComponent<BlockScript>().type != BlockScript.BlockType.Ground)
            {
                return;
            }

            if (_hit.transform.GetComponent<GroundBlockScript>().hasTower)
            {
                return;
            }

            Vector3 hitBlockPosition = _hit.transform.position;
            Vector3 newBlockPosition = hitBlockPosition + new Vector3(0, 2, 0);

            ShowIndicatorCube();
            _indicatorCube.transform.position = newBlockPosition;
            
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                TowerSelectorScript towerSelectorScript = GetComponent<TowerSelectorScript>();

                if (towerSelectorScript.GetSelectedType() == TowerManagerScript.TowerType.Platform)
                {
                    if (!GetComponent<TowerManagerScript>().CreateNewPlatform(_hit.transform.gameObject))
                    {
                        towerSelectorScript.GetSelectedButton().GetComponent<TowerButtonScript>().Shake();
                    }
                    return;
                }

                if (GetComponent<TowerManagerScript>().CreateNewTower(newBlockPosition,
                        _hit.transform.GetComponent<GroundBlockScript>().height))
                {
                    _hit.transform.GetComponent<GroundBlockScript>().hasTower = true;
                }
                else
                {
                    towerSelectorScript.GetSelectedButton().GetComponent<TowerButtonScript>().Shake();
                }
            }
        }
    }

    private void HandleUpgrade()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            if (_hit.transform.CompareTag("Tower"))
            {
                _upgradeUI.GetComponent<UpgradeUIScript>().OpenUpgrade(_hit.transform.gameObject);
                
                ShowUpgradeIndicatorCube();
            }
            else
            {
                _upgradeUI.GetComponent<UpgradeUIScript>().CloseUpgrade();
                HideUpgradeIndicatorCube();
            }
        }
    }

    private void ShowUpgradeIndicatorCube()
    {
        IndicateObject(_hit.transform.gameObject);
                
        if (!_upgradeIndicatorCube.activeInHierarchy)
        {
            _upgradeIndicatorCube.SetActive(true);
        }
    }

    public void HideUpgradeIndicatorCube()
    {
        if (_upgradeIndicatorCube.activeInHierarchy)
        {
            _upgradeIndicatorCube.SetActive(false);
        }
    }

    public void IndicateObject(GameObject block)
    {
        Vector3 blockPosition = block.transform.position;
        Vector3 newBlockPosition = blockPosition + new Vector3(0, 2, 0);
        _upgradeIndicatorCube.transform.position = newBlockPosition;
                
        if (!_upgradeIndicatorCube.activeInHierarchy)
        {
            _upgradeIndicatorCube.SetActive(true);
        }
    }
}
