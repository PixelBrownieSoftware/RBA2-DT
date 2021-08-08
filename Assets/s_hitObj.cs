using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation2.Objects;

public class s_hitObj : o_generic
{
    public TextMesh text;
    public TextMesh textBK;
    public Animator anim;
    public MeshRenderer rendTXT;
    public MeshRenderer rendTXTBk;

    public Sprite enemy_spr;
    public Sprite player_spr;
    public Sprite healSprite;
    public Sprite blockSprite;
    public Sprite buffUp;
    public Sprite buffDown;

    public void MarkDone()
    {
        text.text = "";
        textBK.text = "";
        anim.Play("");
    }

    private new void Start()
    {
        rendTXT = text.GetComponent<MeshRenderer>();
        rendTXTBk = textBK.GetComponent<MeshRenderer>();
    }
    public void PlayAnim(float dmg, string damageType, Color colour)
    {
        rendererObj.color = colour;
        
        switch (damageType) {
            case "heal_hp":
                rendererObj.sprite = healSprite;
                text.text = "" + dmg;
                textBK.text = "" + dmg;
                anim.Play("HealOBJ");
                break;

            case "buffDex":
                rendererObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffAgi":
                rendererObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffStr":
                rendererObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "buffVit":
                rendererObj.sprite = buffUp;
                anim.Play("buff_effect");
                break;

            case "block":
                rendererObj.sprite = blockSprite;
                anim.Play("block_press_turn");
                break;
        }
        print(rendererObj.sprite);

        rendTXT.sortingOrder = 11;
        rendTXTBk.sortingOrder = 10;

        switch (damageType)
        {
            case "heal_sp":
            case "heal_hp":
                text.text = "" + dmg;
                textBK.text = "" + dmg;
                break;

            case "buffDex":
                text.text = "Dex +" + dmg;
                textBK.text = "Dex +" + dmg;
                break;

            case "buffAgi":
                text.text = "Agi +" + dmg;
                textBK.text = "Agi +" + dmg;
                break;

            case "buffStr":
                text.text = "Str +" + dmg;
                textBK.text = "Str +" + dmg;
                break;

            case "buffVit":
                text.text = "Vit +" + dmg;
                textBK.text = "Vit +" + dmg;
                break;

            default:
                text.text = "";
                textBK.text = "";
                break;
        }
    }

    public void PlayAnim(int dmg, bool enemy, Color colour)
    {
        if (enemy)
        {
            rendererObj.color = Color.white;
            anim.Play("HitOBJ_enem");
        }
        else
        {
            rendererObj.color = colour;
            anim.Play("HitOBJ");
        }

        rendTXT.sortingOrder = 11;
        rendTXTBk.sortingOrder = 10;
        text.text = "" + dmg;
        textBK.text = "" + dmg;
    }
}
