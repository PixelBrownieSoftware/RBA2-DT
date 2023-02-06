using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class S_IntegerListener : MonoBehaviour
{
    public CH_Int integerFunction;
    public UnityEvent eventFunction;

    public void OnEnable()
    {
        integerFunction.OnFunctionEvent += IntegerFunction;
    }
    public void OnDisable()
    {
        integerFunction.OnFunctionEvent -= IntegerFunction;
    }

    public void IntegerFunction(int _int) {
        eventFunction.Invoke();
    }
}
