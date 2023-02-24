using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoundation2.Objects;
using TMPro;

public class s_hitObj : o_generic
{
    public TextMeshProUGUI text;
    public Animator anim;
    public Image hitObj;

    public Sprite enemy_spr;
    public Sprite player_spr;
    public Sprite healSprite;
    public Sprite blockSprite;
    public Sprite buffUp;
    public Sprite buffDown;

    public void MarkDone()
    {
        text.text = "";
        anim.Play("");
    }

    public void PlayAnim(float dmg, string damageType, Color colour)
    {
        rendererObj.color = colour;
        
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
                hitObj.sprite = blockSprite;
                anim.Play("block_press_turn");
                break;
        }
        print(hitObj.sprite);

        switch (damageType)
        {
            case "heal_sp":
            case "heal_hp":
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

            default:
                text.text = "";
                break;
        }
    }

    public void PlayAnim(int dmg, bool enemy, Color colour)
    {
        if (enemy)
        {
            hitObj.color = Color.white;
            hitObj.sprite = enemy_spr;
            anim.Play("HitOBJ");
        }
        else
        {
            hitObj.color = colour;
            hitObj.sprite = player_spr;
            anim.Play("HitOBJ");
        }

        text.text = "" + dmg;
    }
}
