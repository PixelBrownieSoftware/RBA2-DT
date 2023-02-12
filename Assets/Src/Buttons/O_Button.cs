using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class O_Button : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;

    public void SetButtonColour(Color colour) { 
        image.color = colour;
    }

    public void SetButonText(string _text) { 
        text.text = _text;
    }
}
