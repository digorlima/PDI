using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderToText : MonoBehaviour
{
    public Slider slider;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void ValueChanged()
    {
        int value = (int)slider.value*100;

        textMesh.text = (float)value/100.0f + "";
    }
}
