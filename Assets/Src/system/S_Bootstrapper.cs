using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class S_Bootstrapper
{
    const string startSceneName = "Main Game";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Excecute() {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            var candidate = SceneManager.GetSceneAt(i);
            if (startSceneName == candidate.name) {
                return;
            }
        }
        SceneManager.LoadSceneAsync(startSceneName, LoadSceneMode.Additive);
    }
}
