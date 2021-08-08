using UnityEngine;
using MagnumFoundation2.System.Core;
using MagnumFoundation2.System;
using MagnumFoundation2.Objects;
using System.Collections.Generic;

[System.Serializable]
public struct s_shopItem {
    public float price;
    public s_move item;
    public s_shopItem(float price, s_move item) {
        this.item = item;
        this.price = price;
    }
}

public class o_locationOverworld : s_object
{
    public string mapName;
    public enum LOCATION_TYPE {
        BATTLE,
        REPLAYABLE_BATTLE,
        BOUNDARY,
        SHOP,
        CUTSCENE_BATTLE,
        TAVERN,
        VIRTUAL_BATTLE
    };
    public LOCATION_TYPE locationType;
    public bool isDone;
    public bool isUnlocked = false;
    public s_enemyGroup group;
    public s_enemyGroup[] groups;
    public s_shopItem[] items;
    public BoxCollider2D bx;
    public List<o_locationOverworld> links;

    public Sprite bigBattle;
    public Sprite replayBattle;
    public Sprite replayBattleHard;
    public Sprite shop;
    public Sprite boundary;

    public void ChangeSprite()
    {
        switch (locationType)
        {
            case LOCATION_TYPE.BOUNDARY:
                rendererObj.sprite = boundary;
                break;

            case LOCATION_TYPE.BATTLE:
            case LOCATION_TYPE.VIRTUAL_BATTLE:
            case LOCATION_TYPE.CUTSCENE_BATTLE:
                rendererObj.sprite = bigBattle;
                break;

            case LOCATION_TYPE.SHOP:
                rendererObj.sprite = shop;
                break;

            case LOCATION_TYPE.REPLAYABLE_BATTLE:
                if (groups.Length > 0)
                {
                    if (groups[0].name[groups[0].name.Length - 1] == 'h')
                        rendererObj.sprite = replayBattleHard;
                    else
                        rendererObj.sprite = replayBattle;
                } else
                {
                    rendererObj.sprite = replayBattle;
                }
                break;
        }
    }

    private new void Start()
    {
        base.Start();
        if (switchA) {
            isDone = true;
        }
    }

    private new void Update()
    {
        ChangeSprite();

        if (links != null) {
            if (links.Count > 0)
            {
                if (links.FindAll(x => x.isDone).Count == links.Count)
                    isUnlocked = true;
            }
        }
        if (isUnlocked)
        {
            switch (locationType)
            {
                case LOCATION_TYPE.BOUNDARY:
                    gameObject.SetActive(false);
                    break;

                case LOCATION_TYPE.REPLAYABLE_BATTLE:
                case LOCATION_TYPE.BATTLE:
                case LOCATION_TYPE.CUTSCENE_BATTLE:
                case LOCATION_TYPE.SHOP:
                case LOCATION_TYPE.VIRTUAL_BATTLE:
                    rendererObj.color = Color.white;
                    bx.gameObject.SetActive(false);
                    break;
            }
        }
        else
        {
            switch (locationType)
            {
                case LOCATION_TYPE.BATTLE:
                    rendererObj.color = new Color(0.6f, 0.6f, 0.6f, 1);
                    break;
            }
        }
        if (isDone)
        {

            switch (locationType)
            {
                case LOCATION_TYPE.BATTLE:
                    rendererObj.color = new Color(1, 1, 1, 0.4f);
                    break;
            }
        }
        o_character c = IfTouchingGetCol<o_character>(collision);
        if (c != null)
        {
            c_player p = c.gameObject.GetComponent<c_player>();
            //print(name + c.name);
            if (p)
            {
                if (Input.GetKeyDown(s_globals.GetKeyPref("select")))
                {
                    switch (locationType) {

                        case LOCATION_TYPE.CUTSCENE_BATTLE:
                            if (!isDone) {
                                s_rpgGlobals.rpgGlSingleton.SetLocationObject(this);
                                UnityEngine.SceneManagement.SceneManager.LoadScene(mapName);
                            }
                            break;

                        case LOCATION_TYPE.BATTLE:
                            if (!isDone)
                                s_rpgGlobals.rpgGlSingleton.SwitchToBattle(group, this);
                            break;

                        case LOCATION_TYPE.VIRTUAL_BATTLE:
                            s_rpgGlobals.rpgGlSingleton.SwitchToBattle(group, this);
                            break;

                        case LOCATION_TYPE.REPLAYABLE_BATTLE:
                            s_rpgGlobals.rpgGlSingleton.SwitchToBattle(groups[Random.Range(0, groups.Length)], this);
                            break;

                        case LOCATION_TYPE.SHOP:
                            if (!switchA) {
                                foreach (s_shopItem it in items) {
                                    s_rpgGlobals.rpgGlSingleton.shopItems.Add(it);
                                }
                                switchA = true;
                                s_globals.GetInstance().AddTriggerState(new triggerState(name, gameObject.scene.name, switchA));
                            }
                            s_menuhandler.GetInstance().GetMenu<s_battleMenu>("OverworldExtraSkill").menType
                                = s_battleMenu.MENU_TYPE.SHOP;
                            s_menuhandler.GetInstance().SwitchMenu("OverworldExtraSkill");
                            break;
                    }
                }
            }
        }
    }
}