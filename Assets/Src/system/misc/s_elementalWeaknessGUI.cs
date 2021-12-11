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
            ELEMENT_WEAKNESS aff = bcD.elementWeakness[el];
            switch (aff) {
                case ELEMENT_WEAKNESS.FRAIL:
                    weakTXT.text = "Frail";
                    weakImg.color = frail;
                    break;

                case ELEMENT_WEAKNESS.NONE:
                    weakTXT.text = "";
                    weakImg.color = normal;
                    break;

                case ELEMENT_WEAKNESS.ABSORB:
                    weakTXT.text = "Abs";
                    weakImg.color = absorb;
                    break;

                case ELEMENT_WEAKNESS.REFLECT:
                    weakTXT.text = "Ref";
                    weakImg.color = reflect;
                    break;

                case ELEMENT_WEAKNESS.NULL:
                    weakTXT.text = "Void";
                    weakImg.color = voidDMG;
                    break;
            }
            weakTXTShadow.text = weakTXT.text;
        }
    }
}
