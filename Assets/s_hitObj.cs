using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.Objects;
using TMPro;

public class s_hitObj : o_generic
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI additionalText;
    public Animator anim;
    public Image hitObj;

    public Sprite enemy_spr;
    public Sprite luck_spr;
    public Sprite player_spr;
    public Sprite healSprite;
    public Sprite blockSprite;
    public Sprite buffUp;
    public Sprite buffDown;
    public Color currentColour = Color.white;

    private new void Update()
    {
        hitObj.color = new Color(currentColour.r, currentColour.g, currentColour.b, hitObj.color.a);
    }


    public void MarkDone()
    {
        text.text = "";
        anim.Play("");
    }

    public void PlayAnim(float dmg, string damageType, Color colour)
    {
        currentColour = colour;
        hitObj.color = new Color(currentColour.r, currentColour.g, currentColour.b, hitObj.color.a);
        switch (damageType) {
            case "heal_hp":
                hitObj.sprite = healSprite;
                text.text = "" + dmg;
                anim.Play("HealOBJ");
                break;

            case "buffDex":
                hitObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffAgi":
                hitObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffStr":
                hitObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffVit":
                hitObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "block":
                anim.Play("block_press_turn");
                break;

            case "miss_attack":
                anim.Play("miss_attack");
                break;

            case "total":
                additionalText.text = "TOTAL";
                anim.Play("totalDmg");
                break;
        }
        switch (damageType)
        {
            case "heal_sp":
            case "heal_hp":
            case "total":
                text.text = "" + dmg;
                break;

            case "buffDex":
                text.text = "Dex +" + dmg;
                break;

            case "buffAgi":
                text.text = "Agi +" + dmg;
                break;

            case "buffStr":
                text.text = "Str +" + dmg;
                break;

            case "buffVit":
                text.text = "Vit +" + dmg;
                break;

            case "block":
                text.text = "VOID!";
                break;

            case "miss_attack":
                text.text = "MISS...";
                break;

            default:
                text.text = "";
                break;
        }
    }

    public void PlayAnim(int dmg, bool enemy, Color colour, string dmgType)
    {
        currentColour = colour;
        hitObj.color = new Color(currentColour.r, currentColour.g, currentColour.b, hitObj.color.a);
        print(hitObj.color);
        if (enemy) {
            switch (dmgType) {
                default:
                    hitObj.sprite = enemy_spr;
                    anim.Play("HitOBJ_enem");
                    break;
                case "lucky":
                    additionalText.text = "LUCKY!";
                    anim.Play("HitOBJ_lucky");
                    break;
                case "critical":
                    additionalText.text = "CRITICAL!";
                    anim.Play("HitOBJ_lucky");
                    break;
            }
        } else {
            switch (dmgType) {
                default:
                    hitObj.sprite = player_spr;
                    anim.Play("HitOBJ");
                    break;
                case "lucky":
                    additionalText.text = "LUCKY!";
                    anim.Play("HitOBJ_lucky");
                    break;
                case "critical":
                    additionalText.text = "CRITICAL!";
                    anim.Play("HitOBJ_lucky");
                    break;
            }
        }
        text.text = "" + dmg;
    }
}
