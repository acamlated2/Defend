using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float rotationSensitivity = 0.1f;
    [SerializeField] private float movementSpeed = 40;
    private float _boostMultiplier = 1;
    private float _slowMultiplier = 1;

    private Vector3 _prevMousePos;
    private bool _isRightClickDragging;
    private bool _isWPressed;
    private bool _isSPressed;
    private bool _isAPressed;
    private bool _isDPressed;
    private bool _isQPressed;
    private bool _isEPressed;
    private bool _isShiftPressed;
    private bool _isControlPressed;

    private void Update()
    {
        CheckInputs();

        if (_isRightClickDragging)
        {
            Vector3 deltaMousePos = Input.mousePosition - _prevMousePos;
            _prevMousePos = Input.mousePosition;

            (deltaMousePos.x, deltaMousePos.y) = (-deltaMousePos.y, deltaMousePos.x);

            Vector3 rotation = transform.localRotation.eulerAngles + deltaMousePos * rotationSensitivity;
            
            Quaternion targetRotation = Quaternion.Euler(rotation);

            transform.localRotation = targetRotation;
        }
        
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_isShiftPressed)
        {
            _boostMultiplier = 2;
        }
        else
        {
            _boostMultiplier = 1;
        }

        if (_isControlPressed)
        {
            _slowMultiplier = 0.5f;
        }
        else
        {
            _slowMultiplier = 1;
        }

        if (_isWPressed)
        {
            transform.position = transform.position + transform.forward * GetTotalSpeed() * Time.deltaTime;
        }
        if (_isSPressed)
        {
            transform.position = transform.position - transform.forward * GetTotalSpeed() * Time.deltaTime;
        }
        if (_isAPressed)
        {
            transform.position = transform.position - transform.right * GetTotalSpeed() * Time.deltaTime;
        }
        if (_isDPressed)
        {
            transform.position = transform.position + transform.right * GetTotalSpeed() * Time.deltaTime;
        }
        if (_isQPressed)
        {
            transform.position = transform.position - transform.up * GetTotalSpeed() * Time.deltaTime;
        }
        if (_isEPressed)
        {
            transform.position = transform.position + transform.up * GetTotalSpeed() * Time.deltaTime;
        }
    }

    private float GetTotalSpeed()
    {
        return movementSpeed * _boostMultiplier * _slowMultiplier;
    }

    private void CheckInputs()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isRightClickDragging = true;
            _prevMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isRightClickDragging = false;
        }
        
        if (Input.GetKeyDown("w"))
        {
            _isWPressed = true;
        }

        if (Input.GetKeyUp("w"))
        {
            _isWPressed = false;
        }
        
        if (Input.GetKeyDown("s"))
        {
            _isSPressed = true;
        }

        if (Input.GetKeyUp("s"))
        {
            _isSPressed = false;
        }
        
        if (Input.GetKeyDown("a"))
        {
            _isAPressed = true;
        }

        if (Input.GetKeyUp("a"))
        {
            _isAPressed = false;
        }
        
        if (Input.GetKeyDown("d"))
        {
            _isDPressed = true;
        }

        if (Input.GetKeyUp("d"))
        {
            _isDPressed = false;
        }
        
        if (Input.GetKeyDown("q"))
        {
            _isQPressed = true;
        }

        if (Input.GetKeyUp("q"))
        {
            _isQPressed = false;
        }
        
        if (Input.GetKeyDown("e"))
        {
            _isEPressed = true;
        }

        if (Input.GetKeyUp("e"))
        {
            _isEPressed = false;
        }
        
        if (Input.GetKeyDown("left shift"))
        {
            _isShiftPressed = true;
        }

        if (Input.GetKeyUp("left shift"))
        {
            _isShiftPressed = false;
        }
        
        if (Input.GetKeyDown("left ctrl"))
        {
            _isControlPressed = true;
        }

        if (Input.GetKeyUp("left ctrl"))
        {
            _isControlPressed = false;
        }
    }
}
