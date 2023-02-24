using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct GachaRanks {
    public string name;
    public GR_Char[] characters;
    public GR_Item[] items;
}
[System.Serializable]
public struct GR_Char
{
    public o_battleCharDataN character;
    public int defeatReq;
    public float rarity;
}
[System.Serializable]
public struct GR_Item
{
    public s_move item;
    public float rarity;
}

public class M_Gacha : S_MenuSystem
{
    public R_Int tokens;
    public R_Items inventory;
    public R_BattleCharacterList players;
    public R_CharacterUnlock defeatedCharacters;
    public R_Int totalRolls;
    public CH_Func backToGachaMenu;
    public CH_Int rollFunc;
    public GachaRanks[] ranks;

    public GameObject[] buttons;
    public B_Function back;

    public GameObject gachaResMenu;
    public B_Function backFromGachaMenu;
    public TMPro.TextMeshProUGUI gachaText;
    public Image gachaImg;

    public R_MoveList common;   //34%
    public R_MoveList uncommon;     //28%
    public R_MoveList rare;     //13%

    private const float commonItemDropChance = 0.64f;
    private const float uncommonItemDropChance = 0.38f;
    private const float rareItemDropChance = 0.13f;
    private const float commonAssetDropChance = 0.73f;
    private const float uncommonAssetDropChance = 0.42f;
    private const float rareItemAssetChance = 0.18f;
    private const float characterChance = 0.15f;
    public R_Float tokenCost;

    int selectedRank = 0;

    private const int pity = 25;    //You'll get a character when you get this

    float GetDropPercentage(float chancePerc)
    {
        float total = commonItemDropChance +
                uncommonItemDropChance +
                rareItemDropChance +
                commonAssetDropChance +
                uncommonAssetDropChance +
                rareItemDropChance +
                characterChance;
        return chancePerc / total;
    }

    private void OnEnable()
    {
        backToGachaMenu.OnFunctionEvent += BackToMenu;
        rollFunc.OnFunctionEvent += Roll;
    }

    private void OnDisable()
    {
        backToGachaMenu.OnFunctionEvent -= BackToMenu;
        rollFunc.OnFunctionEvent -= Roll;
    }

    private void ToggleButtons(bool cond)
    {
        foreach (var a in buttons)
        {
            a.gameObject.SetActive(cond);
        }
        back.gameObject.SetActive(cond);
    }

    public override void StartMenu()
    {
        base.StartMenu();
        gachaResMenu.SetActive(false);
    }

    public void Roll(int num)
    {
        bool canDoToken = tokens.integer >= num;
        if (canDoToken)
        {
            tokens.integer -= num;
        }
        else
        {
            return;
        }
        ToggleButtons(false);
        StartCoroutine(GachaRoll(num));
    }


    public void BackToMenu()
    {
        gachaResMenu.SetActive(false);
        ToggleButtons(true);
    }

    public IEnumerator GachaRoll(int rolls)
    {
        yield return new WaitForSeconds(0.3f);
        /*
        ranks[selectedRank];
        O_BattleCharacter addChara()
        {
            O_BattleCharacter bc = playerSlots.GetChracter(players.characterListRef.Count - 1);
            int heroMinLVL = Hero.level - 5;
            if (heroMinLVL < 1)
                heroMinLVL = 1;
            O_CharacterSetter[] charSetter = scehduele.days[currentDay.integer].stage.characterBases;

            bc.charaName = names.PickRandom();
            bc.Initialise(Random.Range(heroMinLVL, Hero.level + 5), charSetter[Random.Range(0, charSetter.Length)]);
            print(bc.charaName + " has joined!");
            return bc;
        }

        List<O_BattleCharacter> bcs = new List<O_BattleCharacter>();
        Dictionary<O_Move, int> items = new Dictionary<O_Move, int>();
        List<O_Asset> assets = new List<O_Asset>();

        for (int i = 0; i < rolls; i++)
        {
            totalRolls.integer++;
            O_Move item = null;
            O_Asset asset = null;
            O_BattleCharacter bc = null;

            if (totalRolls.integer % pity == 0)
            {
                if (players.characterListRef.Count < playerSlots.characterListRef.Count)
                    bc = addChara();
                else
                    item = rare.PickRandom();
            }
            else
            {
                float percentage = Random.Range(0f, 1f);
                if (percentage < GetDropPercentage(commonItemDropChance))
                {
                    item = common.PickRandom();
                }
                else if (percentage < GetDropPercentage(uncommonItemDropChance))
                {
                    item = uncommon.PickRandom();
                }
                else if (percentage < GetDropPercentage(rareItemDropChance))
                {
                    item = rare.PickRandom();
                }
                else if (percentage < GetDropPercentage(commonAssetDropChance))
                {
                    asset = commonAssets.PickRandom();
                }
                else if (percentage < GetDropPercentage(uncommonAssetDropChance))
                {
                    asset = uncommonAssets.PickRandom();
                }
                else if (percentage < GetDropPercentage(rareItemAssetChance))
                {
                    asset = rareAssets.PickRandom();
                }
                else if (percentage < GetDropPercentage(characterChance))
                {
                    if (players.characterListRef.Count < playerSlots.characterListRef.Count)
                        bc = addChara();
                    else
                        item = rare.PickRandom();
                }
                else
                {
                    asset = commonAssets.PickRandom();
                }

            }
            if (bc != null)
            {
                players.Add(bc);
                bcs.Add(bc);
            }
            if (item != null)
            {
                inventory.moveListRef.Add(item);
                if (items.ContainsKey(item))
                {
                    items[item]++;
                }
                else
                {
                    items.Add(item, 1);
                }
            }
            if (asset != null)
            {
                asset.amount++;
                assets.Add(asset);
            }
        }
        string assetText = "";
        gachaText.text = assetText;
        foreach (var a in assets)
        {
            assetText += a.name + "\n";
        }
        gachaText.text += assetText;
        string itemText = "";
        foreach (var i in items)
        {
            itemText += i.Key.name + " x " + i.Value + "\n";
        }
        gachaText.text += itemText;
        string characterText = "";
        foreach (var ch in bcs)
        {
            characterText += ch.charaName + " was recruited into the party." + "\n";
        }
        gachaText.text += characterText;
        gachaResMenu.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        */
    }
}