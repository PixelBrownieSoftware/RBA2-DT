using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_elementalWeaknessGUI : MonoBehaviour
{
    public Image weakImg;
    public Text weakTXT;
    public Text weakTXTShadow;
    o_battleCharPartyData bcD;
    public ELEMENT el;

    public Color normal;
    public Color frail;
    public Color voidDMG;
    public Color resist;
    public Color absorb;
    public Color reflect;

    public void SetToDat(o_battleCharPartyData pd) {
        bcD = pd;
    }

    void Update()
    {
        if (bcD != null) {
            float aff = bcD.elementAffinities[(int)el];
            if (aff > 1.999f)
            {
                weakTXT.text = "Frail";
                weakImg.color = frail;
            }
            else if (aff < 2 && aff >= 1)
            {
                weakTXT.text = "";
                weakImg.color = normal;
            }
            else if (aff < 1 && aff> 0) {

                weakTXT.text = "Res";
                weakImg.color = resist;
            }
            else if (aff == 0)
            {
                weakTXT.text = "Void";
                weakImg.color = voidDMG;
            }
            else if (aff < 0 && aff > -1.999f)
            {
                weakTXT.text = "Ref";
                weakImg.color = reflect;
            }
            else if (aff < -2)
            {
                weakTXT.text = "Abs";
                weakImg.color = absorb;
            }
            weakTXTShadow.text = weakTXT.text;
        }
    }
}
