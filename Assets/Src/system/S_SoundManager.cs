using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundManager : MonoBehaviour
{
    public AudioSource audioSrc;
    public CH_SoundPitch soundPitch;

    private void OnEnable()
    {
        soundPitch.OnFunctionEvent += PlaySound;
    }

    public void PlaySound(AudioClip sound, float pitch) {
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(sound);
    }
}
