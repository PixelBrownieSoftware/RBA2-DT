using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle animation")]
public class o_animclip : ScriptableObject
{
    [System.Serializable]
    public struct animCL {
        public float timer;
        public Sprite sprite;
    }
    public animCL[] animation;
}
