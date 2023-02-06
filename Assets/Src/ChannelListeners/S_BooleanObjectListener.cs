using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BooleanObjectListener : MonoBehaviour
{
    public CH_Boolean listener;
    public GameObject[] enablingObjects;

    public void OnEnable()
    {
        listener.OnFunctionEvent += DisableObject;
    }

    public void OnDisable()
    {
        listener.OnFunctionEvent -= DisableObject;
    }

    public void DisableObject(bool _condition) {
        foreach (var a in enablingObjects)
        {
            a.gameObject.SetActive(_condition);
        }
    }
}
