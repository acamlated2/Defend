using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private Camera _camera;

    private GameObject _indicatorCube;

    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _camera = Camera.main;

        _indicatorCube = GameObject.FindGameObjectWithTag("IndicatorCube");
    }

    private void Update()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform.tag != "Block")
            {
                HideIndicatorCube();
                return;
            }

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
                GetComponent<TowerManagerScript>().CreateNewTower(newBlockPosition);
                _hit.transform.GetComponent<GroundBlockScript>().hasTower = true;
            }
        }
        else
        {
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
}
