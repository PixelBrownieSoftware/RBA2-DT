using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class O_ProjectileAnim : MonoBehaviour
{
    public Animator anim;
    public GameObject subObj;
    public IObjectPool<O_ProjectileAnim> pool;
    public CH_SoundPitch soundPlay;

    public void DespawnObject()
    {
        if (pool != null)
            pool.Release(this);
    }
    public void PlaySound(AudioClip cl)
    {
        soundPlay.RaiseEvent(cl, 1);
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
