using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CHL_ChangeText : MonoBehaviour
{
    [SerializeField]
    private CH_Text textEvent;
    [SerializeField]
    private TextMeshProUGUI textObj;

    public void ChangeText(string text) {
        textObj.text = text;
    }

    private void OnEnable()
    {
        textEvent.OnTextEventRaised += ChangeText;
    }

    private void OnDisable()
    {
        textEvent.OnTextEventRaised -= ChangeText;
    }
}
