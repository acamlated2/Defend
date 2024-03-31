using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyUIScript : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void ChangeValue(float newAmount)
    {
        _text.text = "$" + newAmount;
    }
}
