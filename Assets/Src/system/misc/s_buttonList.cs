using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class s_buttonList : MonoBehaviour
{

    public void ForwardPage() {

    }

    public List<s_button> listOfButtons = new List<s_button>();
    
    public s_button GetButton(int i) {
        listOfButtons[i].gameObject.SetActive(true);
        return listOfButtons[i];
    }

    public void ResetButtons() {
        listOfButtons.ForEach(x => x.gameObject.SetActive(false));
    }
}
