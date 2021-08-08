using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_rpganim : MonoBehaviour
{
    public o_animclip currentClip;
    public SpriteRenderer render;
    float timer;
    int index;
    public bool done = false;
    public bool isloop;

    public void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void PlayAnimation(o_animclip Clip, bool loop) {
        isloop = loop;
        currentClip = Clip;
        done = false;
    }

    public void Update()
    {
        if (currentClip != null) {
            if (!done) {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else {
                    if (index > currentClip.animation.Length - 2) {
                        index = 0;
                        if (!isloop)
                            done = true;
                        else
                            timer = currentClip.animation[index].timer;
                    } else {
                        index++;
                        timer = currentClip.animation[index].timer;
                    }
                }
                render.sprite = currentClip.animation[index].sprite;
            }
        }
    }
}
