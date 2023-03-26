using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class s_hpBoxGUI : MonoBehaviour
{
    public Text comboText;
    public Image comboImage;
    public GameObject comboObj;
    public Image HPboxColour;
    public Image StatusEff;

    public Text hpText;
    public Text spText;
    Text hpTextShadow;
    Text spTextShadow;

    public o_battleCharacter bc;

    public Slider hpSlider;
    public Slider spSlider;

    public Image hpBR;
    public Image spBR;
    public Image HPPic;
    public Image Arrow;
    public Material mat;
    public MaterialPropertyBlock mt;
    public Renderer rend;

    public GameObject ob;

    public R_Character currentCharacter;

    public Sprite poisionIcon;
    public Sprite stunIcon;
    public Sprite confuseIcon;
    public Sprite defaultImage;

    float statusTimer = 0f;
    const float statusFlipTimer = 0.5f;
    int statusFlipIndex = 0;
    int prevStatusFlipCount = 0;

    private void Awake()
    {
        hpTextShadow = hpText.transform.GetChild(0).GetComponent<Text>();
        spTextShadow = spText.transform.GetChild(0).GetComponent<Text>();
        HideComboImage();
    }

    public void SetMaterial() {
        if (bc == null)
        {
            ob.SetActive(false);
        } else
        {
            ob.SetActive(true);
            HPPic.color = bc.battleCharData.characterColour;
            if (bc.battleCharData.battleImage != null)
            {
                HPPic.sprite = bc.battleCharData.battleImage;
            }
            else
            {
                HPPic.sprite = defaultImage;
            }
        }
    }


    public void Update()
    {
        if (bc != null)
        {
            if (bc.referencePoint == currentCharacter.characterRef)
            {
                Arrow.gameObject.SetActive(true);
            }
            else
            {
                Arrow.gameObject.SetActive(false);
            }
            ob.SetActive(true);
            hpSlider.value = ((float)bc.health / (float)bc.maxHealth) * 100;
            spSlider.value = ((float)bc.stamina / (float)bc.maxStamina) * 100;
            hpBR.color = bc.battleCharData.characterColour;
            spBR.color = bc.battleCharData.characterColour2;

            hpText.text = "" + bc.health;
            hpTextShadow.text = "" + bc.health;
            spText.text = "" + bc.stamina;
            spTextShadow.text = "" + bc.stamina;

            #region STATUS EFFECTS
            List<string> statusEffs = new List<string>();
            foreach (s_statusEff stat in bc.statusEffects) {
                /*
                switch (stat.status) {
                    case STATUS_EFFECT.POISON:
                        statusEffs.Add("psn");
                        break;
                    case STATUS_EFFECT.STUN:
                        statusEffs.Add("stn");
                        break;

                    case STATUS_EFFECT.CONFUSED:
                        statusEffs.Add("con");
                        break;
                }
                */
            }
            if (statusEffs.Count > 0)
            {
                if (prevStatusFlipCount != statusEffs.Count)
                {
                    statusFlipIndex = 0;
                }
                if (statusTimer > 0)
                {
                    statusTimer -= Time.deltaTime;
                }
                else
                {
                    statusTimer = statusFlipTimer;
                    if (statusFlipIndex < statusEffs.Count - 1)
                        statusFlipIndex++;
                    else
                        statusFlipIndex = 0;
                }
                switch (statusEffs[statusFlipIndex])
                {
                    case "psn":
                        StatusEff.sprite = poisionIcon;
                        break;
                    case "stn":
                        StatusEff.sprite = stunIcon;
                        break;
                    case "con":
                        StatusEff.sprite = confuseIcon;
                        break;
                }
                StatusEff.color = Color.white;
            }
            else {
                StatusEff.color = Color.clear;
            }
            
            #endregion


        }
        else {

            Arrow.gameObject.SetActive(false);
            ob.SetActive(false);
        }
    }

    public void ChangeComboImage(s_move mov) {
        if (mov != null)
        {
            comboObj.SetActive(true);
            comboText.text = mov.name;
        }
    }
    public void HideComboImage()
    {
        comboObj.SetActive(false);
    }
}
