using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private string textString;
    private TMP_Text _mainText;

    private TMP_Text _counterText;
    
    private Slider _slider;
    public float sliderValue;


    private void Awake()
    {
        _mainText = transform.GetChild(0).GetComponent<TMP_Text>();
        _mainText.text = textString;

        _counterText = transform.GetChild(1).GetComponent<TMP_Text>();
        _counterText.text = sliderValue.ToString();

        _slider = transform.GetChild(2).GetComponent<Slider>();
    }

    private void Update()
    {
        sliderValue = _slider.value;
        _counterText.text = Math.Round(sliderValue, 2).ToString();
    }
}
