using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneLoader : MonoBehaviour
{
    [SerializeField] private S_Transition m_transitionSO;
    [SerializeField] private CH_MapTransfer m_sceneLoadEvent;
    [SerializeField] private CH_Func m_sceneLoadedEventChannel;
    [SerializeField] private CH_Fade m_FadeChannel;
    AsyncOperation operationSceneLoad;

    string currentSceneLoad; 

    private void OnDisable()
    {
        m_sceneLoadEvent.OnMapTransferEvent -= LoadScene;
        if(operationSceneLoad != null)
            operationSceneLoad.completed -= SceneLoaded;
    }

    private void OnEnable()
    {
        print("OK");
        m_sceneLoadEvent.OnMapTransferEvent += LoadScene;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void SceneLoaded(AsyncOperation obj) {
        Scene activeScene = SceneManager.GetSceneByName(m_transitionSO._currentLocation);
        SceneManager.SetActiveScene(activeScene);
        m_sceneLoadedEventChannel.RaiseEvent();
        m_FadeChannel.Fade(Color.clear);
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        string m_prevScene = m_transitionSO._currentLocation;
        m_FadeChannel.Fade(Color.black);
        m_transitionSO._prevLocation = m_prevScene;
        yield return new WaitForSeconds(0.75f);
        if (m_transitionSO._prevLocation != "")
        {
            SceneManager.UnloadSceneAsync(m_transitionSO._prevLocation);
        }
        operationSceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operationSceneLoad.completed += SceneLoaded;
        m_transitionSO._currentLocation = sceneName;
    }
}
