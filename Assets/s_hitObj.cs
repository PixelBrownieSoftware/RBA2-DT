using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using MagnumFoundation2.Objects;
using MagnumFoundation2.System.Core;
using TMPro;

public class s_hitObj : MonoBehaviour
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
    public IObjectPool<s_hitObj> pool;

    private void Update()
    {
        hitObj.color = new Color(currentColour.r, currentColour.g, currentColour.b, hitObj.color.a);
    }

    public void PlaySound(AudioClip cl)
    {
        s_soundmanager.GetInstance().PlaySound(cl);
    }

    public void MarkDone()
    {
        text.text = "";
        anim.Play("");
    }

    public void PlayAnim(int dmg, string damageType, Color colour)
    {
        print(damageType);
        currentColour = colour;
        hitObj.color = new Color(currentColour.r, currentColour.g, currentColour.b, hitObj.color.a);
        switch (damageType)
        {
            case "damage_player":
                print(dmg);
                text.text = "" + dmg;
                hitObj.sprite = player_spr;
                anim.Play("HitOBJ");
                break;
            case "damage_enemy":
                print(dmg);
                text.text = "" + dmg;
                hitObj.sprite = enemy_spr;
                anim.Play("HitOBJ_enem");
                break;
            case "lucky":
                text.text = "" + dmg;
                additionalText.text = "LUCKY!";
                anim.Play("HitOBJ_lucky");
                break;
            case "critical":
                text.text = "" + dmg;
                additionalText.text = "CRITICAL!";
                anim.Play("HitOBJ_lucky");
                break;
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
                text.text = "VOID!";
                anim.Play("block_press_turn");
                break;
            case "miss_attack":
                text.text = "MISS...";
                anim.Play("miss_attack");
                break;
            case "total":
                additionalText.text = "TOTAL";
                anim.Play("totalDmg");
                break;
        }
    }

    public void DespawnObject() {
        if (pool != null)
            pool.Release(this);
    }
}
