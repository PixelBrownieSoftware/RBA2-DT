using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

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
    public S_RPGGlobals RPGGlobal;
    public R_Int tokens;
    public R_Items inventory;
    public R_BattleCharacterList players;
    public R_CharacterUnlock defeatedCharacters;
    public R_Int totalRolls;
    public CH_Func backToGachaMenu;
    public CH_Func rollFunc;
    public CH_Int selectRank;
    public GachaRanks[] ranks;
    public GachaRanks currentRank;

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
    public B_Function rollButton;

    public Slider tokenSlider;

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
        selectRank.OnFunctionEvent += SelectRank;
        backToGachaMenu.OnFunctionEvent += BackToMenu;
        rollFunc.OnFunctionEvent += Roll;
    }

    private void OnDisable()
    {
        selectRank.OnFunctionEvent -= SelectRank;
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
        currentRank = ranks[0];
        base.StartMenu();
        gachaResMenu.SetActive(false);
    }

    public void SelectRank(int i) {
        currentRank = ranks[i];
    }

    public void Roll()
    {
        bool canDoToken = tokens.integer >= tokenSlider.value;
        if (canDoToken)
        {
            tokens.integer -= (int)tokenSlider.value;
        }
        else
        {
            return;
        }
        ToggleButtons(false);
        StartCoroutine(GachaRoll((int)tokenSlider.value));
    }

    private void Update()
    {
        tokenSlider.maxValue = tokens.integer;
        rollButton.SetButonText(tokenSlider.value + " tokens.");
    }

    public void BackToMenu()
    {
        gachaResMenu.SetActive(false);
        ToggleButtons(true);
    }

    public IEnumerator GachaRoll(int rolls)
    {
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
        */
        o_battleCharDataN bc = null;
        Dictionary<o_battleCharDataN, int> bcs = new Dictionary<o_battleCharDataN, int>();
        Dictionary<s_move, int> items = new Dictionary<s_move, int>();
        gachaText.text = "";
        for (int i = 0; i < rolls; i++)
        {
            totalRolls.integer++;
            s_move item = null;

            if (totalRolls.integer % pity == 0)
            {
                float percentage = Random.Range(0f, 1f);
                List<GR_Char> characterRarity = new List<GR_Char>();
                foreach (var ob in currentRank.characters)
                {
                    if (percentage < ob.rarity)
                    {
                        characterRarity.Add(ob);
                    }
                }
                if (characterRarity.Count > 0)
                {
                    bc = characterRarity[Random.Range(0, characterRarity.Count)].character;
                }
                else
                {
                    List<GR_Char> characterForce = currentRank.characters.ToList<GR_Char>();
                    float max = 0;
                    foreach (var charPick in characterForce) {
                        if (charPick.rarity > max) {
                            max = charPick.rarity;
                            bc = charPick.character;
                        }
                    }
                }
                if (!bcs.ContainsKey(bc))
                    bcs.Add(bc, 1);
                else
                    bcs[bc]++;
            }
            else
            {
                float percentage = Random.Range(0f, 1f);
                List<GR_Item> itemsRarity = new List<GR_Item>();
                foreach (var ob in currentRank.items) {
                    if (percentage < ob.rarity) {
                        itemsRarity.Add(ob);
                    }
                }
                if (itemsRarity.Count > 0)
                    item = itemsRarity[Random.Range(0, itemsRarity.Count)].item;
                else
                {
                    List<GR_Item> itemForce = currentRank.items.ToList<GR_Item>();
                    float max = 0;
                    foreach (var itemPick in itemForce) {
                        if (itemPick.rarity > max) {
                            max = itemPick.rarity;
                            item = itemPick.item;
                        }
                    }
                }
            }
            if (item != null)
            {
                if(!items.ContainsKey(item))
                    items.Add(item, 1);
                else
                    items[item]++;
            }
        }
        string itemText = "";
        foreach (var it in items)
        {
            itemText += it.Key.name + " x " + it.Value + "\n";
            for (int i = 0; i < it.Value; i++)
            {
                inventory.AddItem(it.Key);
            }
        }
        gachaText.text += itemText;
        string characterText = "";

        foreach (var ch in bcs)
        {
            if (!RPGGlobal.ContainsPartyMember(ch.Key))
            {
                RPGGlobal.AddPartyMember(ch.Key, 1);
                if (ch.Value > 1)
                {
                    for (int i = 0; i < ch.Value; i++)
                    {
                        RPGGlobal.AddExpToPartyMember(ch.Key, 0.25f);
                    }
                    characterText += ch.Key.name + " was recruited into the party with some boosts." + "\n";
                }
                else
                {
                    characterText += ch.Key.name + " was recruited into the party." + "\n";
                }
            }
            else
            {
                for (int i = 0; i < ch.Value; i ++) {
                    RPGGlobal.AddExpToPartyMember(ch.Key, 0.25f);
                }
                characterText += ch.Key.name + " got stronger..." + "\n";
            }
        }
        gachaText.text += characterText;
        gachaResMenu.SetActive(true);
        yield return new WaitForSeconds(0.3f);
    }
}