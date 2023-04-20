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
    public S_Element el;

    public Color normal;
    public Color frail;
    public Color voidDMG;
    public Color resist;
    public Color absorb;
    public Color reflect;
    float elementWeakness;
    ELEMENT_WEAKNESS aff;
    public R_BattleCharacter currentCharacter;
    public CH_Func setData;
    public S_EnemyWeaknessReveal revealAffinity;
    bool showAffinity = true;

    private void OnEnable()
    {
        setData.OnFunctionEvent += SetToDat;
    }

    private void OnDisable()
    {
        setData.OnFunctionEvent -= SetToDat;
    }

    public void SetToDat() {
        showAffinity = true;
        bcD = currentCharacter.battleCharacter;
        elementWeakness = bcD.GetElementWeakness(el);
        if (revealAffinity != null) {
            showAffinity = revealAffinity.EnemyWeaknessExists(bcD.characterDataSource, el);
        }
        if (showAffinity)
        {
            if (elementWeakness >= 2)
                aff = ELEMENT_WEAKNESS.FRAIL;
            else if (elementWeakness < 2 && elementWeakness > 0)
                aff = ELEMENT_WEAKNESS.NONE;
            else if (elementWeakness == 0)
                aff = ELEMENT_WEAKNESS.NULL;
            else if (elementWeakness < 0 && elementWeakness > -1)
                aff = ELEMENT_WEAKNESS.REFLECT;
            else if (elementWeakness <= -1)
                aff = ELEMENT_WEAKNESS.ABSORB;
        }
    }

    void Update()
    {
        if (bcD != null)
        {
            if (showAffinity)
            {
                switch (aff)
                {
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
            }
            else
            {
                weakTXT.text = "???";
                weakImg.color = normal;
            }
            weakTXTShadow.text = weakTXT.text;
        }
    }
}
