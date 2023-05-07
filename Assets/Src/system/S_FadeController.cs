using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_FadeController : MonoBehaviour
{
    [SerializeField]
    private Image m_fadeImage;
    [SerializeField] private CH_Fade m_FadeChannel;

    private void OnEnable()
    {
        m_FadeChannel.OnFadeColourEventRaised += FadeOutFunc;
    }

    private void OnDisable()
    {
        m_FadeChannel.OnFadeColourEventRaised -= FadeOutFunc;
    }

    public void FadeOutFunc(Color colour) {
        StartCoroutine(FadeOut(colour));
    }

    public IEnumerator FadeOut(Color colour)
    {
        float t = 0;
        m_fadeImage.gameObject.SetActive(true);
        Color col = m_fadeImage.color;
        m_fadeImage.enabled = true;
        while (m_fadeImage.color != colour)
        {
            m_fadeImage.color = Color.Lerp(col, colour, t);
            t += Time.deltaTime * 15;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (colour == Color.clear) {
            m_fadeImage.enabled = false;
            m_fadeImage.gameObject.SetActive(false);
        }
    }
}
