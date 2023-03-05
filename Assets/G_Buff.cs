using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class G_Buff : MonoBehaviour
{
    public enum GRAPHIC_BUFF_STAT { 
    STR,
    VIT,
    DEX,
    LUC,
    INT,
    AGI
    }
    public o_battleCharacter character;
    public TextMeshProUGUI number;
    public GRAPHIC_BUFF_STAT buffType;
    public Image arrow;

    void Update()
    {
        if (character != null)
        {
            int buff = 0;
            switch (buffType)
            {
                case GRAPHIC_BUFF_STAT.AGI:
                    buff = character.agilityBuff;
                    break;
                case GRAPHIC_BUFF_STAT.STR:
                    buff = character.strengthBuff;
                    break;
                case GRAPHIC_BUFF_STAT.VIT:
                    buff = character.vitalityBuff;
                    break;
                case GRAPHIC_BUFF_STAT.INT:
                    buff = character.intelligenceBuff;
                    break;
                case GRAPHIC_BUFF_STAT.LUC:
                    buff = character.luckBuff;
                    break;
                case GRAPHIC_BUFF_STAT.DEX:
                    buff = character.dexterityBuff;
                    break;
            }
            number.text = "" + Mathf.Abs(buff);
            if (buff < 0)
            {
                arrow.color = Color.red;
                arrow.transform.rotation = Quaternion.Euler(0, 0, -180);
            }
            else
            {
                arrow.color = Color.green;
                arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
