using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class M_CharacterStatus : S_MenuSystem
{
    public s_guiList str;
    public s_guiList dx;
    public s_guiList vit;
    public s_guiList agi;

    public Text strTXT;
    public Text dxTXT;
    public Text vitTXT;
    public Text agiTXT;

    public Text strShadTXT;
    public Text dxShadTXT;
    public Text vitShadTXT;
    public Text agiShadTXT;

    public o_battleCharPartyData characterData;
    public Text health;
    public Text stamina;
    public Text nameCharacter;
    bool isDirty = true;
    public List<s_elementalWeaknessGUI> affs = new List<s_elementalWeaknessGUI>();

    public void SetCharacter(ref o_battleCharPartyData ch)
    {
        isDirty = true;
        characterData = ch;
    }

    public override void OnOpen()
    {
        base.OnOpen(); ResetButton();
    }

    private void Update()
    {
        if (characterData != null)
        {
            if (isDirty)
            {
                health.text = "" + characterData.maxHealth;
                stamina.text = "" + characterData.maxStamina;
                nameCharacter.text = characterData.name;
                str.amount = characterData.strength;
                dx.amount = characterData.dexterity;
                agi.amount = characterData.agility;
                vit.amount = characterData.vitality;

                strTXT.text = strShadTXT.text = "" + characterData.strength;
                vitTXT.text = vitShadTXT.text = "" + characterData.vitality;
                dxTXT.text = dxShadTXT.text = "" + characterData.dexterity;
                agiTXT.text = agiShadTXT.text = "" + characterData.agility;

                //strike_aff.text = characterData.wea
                affs.ForEach(x => x.SetToDat(characterData));
                {
                    int i = 0;
                    foreach (s_move m in characterData.currentMoves)
                    {
                        s_button b = GetButton<s_button>(i);
                        b.txt.text = m.name;
                        i++;
                    }
                }
                isDirty = false;
            }
        }
    }

    public void SetButton(s_button b, int i, List<s_move> mov)
    {
        b.txt.text = mov[i].name;
    }
}

public class M_CharacterStatus : S_MenuSystem
{
    public R_Character selectedCharacter;
    public R_CharacterList players;
    public R_CharacterList selectedCharacters;
    public R_Move selectedMove;
    public CH_BattleCharacter selected;
    public CH_Text changeMenu;
    public R_Boolean isItem;
    public CH_Move moveEvent;

    public Slider hp;
    public Slider mp;
    public Slider exp;
    public GameObject mpObj;
    public B_BattleMove[] moves;

    private void OnEnable()
    {
        moveEvent.OnMoveFunctionEvent += SelectMove;
    }

    private void OnDisable()
    {
        moveEvent.OnMoveFunctionEvent -= SelectMove;
    }

    public void SelectMove(O_Move move)
    {
        if (selectedCharacter.characterRef.mana <= selectedMove.move.mpCost)
        {
            return;
        }
        selectedCharacters.characterListRef.Clear();
        switch (move.moveElement.name)
        {
            default:
                return;
            case "Recovery":
                selectedCharacters.characterListRef.AddRange(players.characterListRef.FindAll(x => x.health < x.maxHealth));
                break;
            case "Cure":
                selectedCharacters.characterListRef.AddRange(players.characterListRef.FindAll(x => x.statusEffects.ContainsKey(move.statusFX)));
                break;
        }
        isItem.boolean = false;
        selectedMove.move = move;
        changeMenu.RaiseEvent("PartyMenuHeal");
    }

    public override void StartMenu()
    {
        foreach (var move in moves) {
            move.gameObject.SetActive(false);
        }
        base.StartMenu();
        for (int i = 0; i < selectedCharacter.characterRef.moves.Count; i++) {
            var move = moves[i];
            var chMove = selectedCharacter.characterRef.moves[i];
            move.gameObject.SetActive(true);
            move.SetButonText(chMove.name);
            move.SetBattleButton(chMove);

        }
        if (selectedCharacter.characterRef.maxMana > 0)
        {
            mp.value = ((float)selectedCharacter.characterRef.mana/ (float)selectedCharacter.characterRef.maxMana);
            mpObj.SetActive(true);
        }
        else
        {
            mpObj.SetActive(false);
        }
        hp.value = ((float)selectedCharacter.characterRef.health / (float)selectedCharacter.characterRef.maxHealth);
        exp.value = (float)selectedCharacter.characterRef.expereince / 1f;
    }

    public void SelectCharacter(O_BattleCharacter bc)
    {
        selectedCharacter.SetCharacter(bc);
        changeMenu.RaiseEvent("PartyMenu");
    }
}
*/