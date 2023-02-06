using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BattleDefault : S_MenuSystem
{
    public R_Character currentCharacter;
    public B_BattleMove attackButton;
    public B_String skillButton;
    public B_String itemButton;

    private void Awake()
    {
        attackButton.gameObject.SetActive(false);
        skillButton.gameObject.SetActive(false);
    }

    public override void StartMenu()
    {
        attackButton.gameObject.SetActive(false);
        skillButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);

        base.StartMenu();

        attackButton.gameObject.SetActive(true);
        if (currentCharacter.characterRef) {
            skillButton.gameObject.SetActive(true);
        }
        if (currentCharacter.characterRef.name == "Hero") {
            itemButton.gameObject.SetActive(true);
        }
    }

}
