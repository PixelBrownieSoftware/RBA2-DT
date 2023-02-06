using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SpriteListener : MonoBehaviour
{
    public CH_Sprite spriteEvent;
    SpriteRenderer _rend;

    void Awake() {
        _rend = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        spriteEvent.OnFunctionEvent -= SpriteSet;
    }

    private void OnEnable()
    {
        spriteEvent.OnFunctionEvent += SpriteSet;
    }

    public void SpriteSet(Sprite _spr)
    {
        _rend.sprite = _spr;
    }
}
