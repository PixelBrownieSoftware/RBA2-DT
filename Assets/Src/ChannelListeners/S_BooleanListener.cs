using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_BooleanListener : MonoBehaviour
{
    public CH_Boolean listener;
    public Button button;
    public Image img;
    [SerializeField]
    private Color enabledColour = Color.white;
    [SerializeField]
    private Color disabledColour = Color.clear;

    public void OnEnable()
    {
        listener.OnFunctionEvent += ToggleButton;
    }

    public void OnDisable()
    {
        listener.OnFunctionEvent -= ToggleButton;
    }

    public void ToggleButton(bool _condition)
    {
        if (img != null) {
            img.color = _condition ? enabledColour : disabledColour; 
        }
        button.enabled = _condition;
    }
}
