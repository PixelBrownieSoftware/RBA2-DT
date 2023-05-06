using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using MagnumFoundation2.System.Core;

public class s_moveanim : MonoBehaviour
{
    public Animator anim;
    public GameObject subObj;
    public IObjectPool<s_moveanim> pool;

    public void DespawnObject()
    {
        if (pool != null)
            pool.Release(this);
    }
    public void PlaySound(AudioClip cl)
    {
        s_soundmanager.GetInstance().PlaySound(cl);
    }

    public void StopAnimation()
    {
        anim.Play("");
        transform.localScale = new Vector3(1, 1, 1);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        subObj.transform.localScale = new Vector3(1, 1, 1);
        subObj.transform.localEulerAngles = new Vector3(0, 0, 0);
        DespawnObject();
    }
}
