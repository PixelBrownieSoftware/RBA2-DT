using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_menuButton : MonoBehaviour
{
    public Image button;
    public Image selector;
    public Text buttonTxt;

    public void SelectorSwitch(bool isOn) {
        if (isOn)
            selector.color = new Color(1, 1, 1, 0.5f);
        else
            selector.color = Color.clear;
    }

    public void SetProperties(Image img, string txt) {
        buttonTxt.text = txt;
    }
}
