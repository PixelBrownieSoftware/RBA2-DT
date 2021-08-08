using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.Objects;

public class c_player : o_character
{
    public bool ProcessMove;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    new void Start()
    {
        DisableAttack();
        Initialize();
        base.Start();
    }

    public new void Update()
    {
        base.Update();
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                if (ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:
                if (control)
                {
                    if (!ArrowKeyControl())
                    {
                        CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
                    }
                }
                break;
        }
    }
}
