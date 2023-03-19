using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_BattleMainMenu : S_MenuSystem
{
    [SerializeField]
    private R_Character currentCharacterRef;
    private CH_BattleChar currentCharacter;
    [SerializeField]
    private R_Character targetCharacterRef;
    [SerializeField]
    private R_MoveList currentMovesRef;
    [SerializeField]
    private R_Move currentMoveRef;
    public B_Function[] buttons;

    public s_move meleeAttack;
    public s_move rangedAttack;

    public s_move defaultAttack;
    public s_move guard;
    public s_move pass;

    [SerializeField]
    private CH_Text changeMenu;

    [SerializeField]
    private R_Boolean isItem;
    [SerializeField]
    private CH_Func goToMelee;
    [SerializeField]
    private CH_Func goToSkills;
    [SerializeField]
    private CH_Func goToPass;
    [SerializeField]
    private CH_Func goToGuard;
    [SerializeField]
    private CH_Func performMove;

    private void OnEnable()
    {
        goToSkills.OnFunctionEvent += GoToSkills;
        goToMelee.OnFunctionEvent += MeleeAttack;
        goToPass.OnFunctionEvent += PassAction;
        goToGuard.OnFunctionEvent += GuardAction;
    }

    private void OnDisable()
    {
        goToSkills.OnFunctionEvent -= GoToSkills;
        goToMelee.OnFunctionEvent -= MeleeAttack;
        goToPass.OnFunctionEvent -= PassAction;
        goToGuard.OnFunctionEvent -= GuardAction;
    }


    public void MeleeAttack()
    {
        if (meleeAttack != null)
            currentMoveRef.SetMove(meleeAttack);
        else
            currentMoveRef.SetMove(defaultAttack);
        changeMenu.RaiseEvent("TargetMenu");
    }

    public void GoToSkills()
    {
        print(currentCharacterRef.characterRef.characterData.currentMoves.Count);
        currentMovesRef.SetMoves(currentCharacter.GetAllMoves());
        isItem.boolean = false;
        changeMenu.RaiseEvent("BattleSkillMenu");
    }
    public void GuardAction()
    {
        targetCharacterRef.SetCharacter(currentCharacter);
        currentMoveRef.SetMove(guard);
        performMove.RaiseEvent();
        changeMenu.RaiseEvent("EMPTY");
    }
    public void PassAction()
    {
        targetCharacterRef.SetCharacter(currentCharacter);
        currentMoveRef.SetMove(pass);
        performMove.RaiseEvent();
        changeMenu.RaiseEvent("EMPTY");
    }


    public override void StartMenu()
    {
        currentCharacter = currentCharacterRef.characterRef;
        if (currentCharacter.physWeapon != null)
            meleeAttack = currentCharacter.physWeapon;
        else
            meleeAttack = null;
        if (currentCharacter.rangedWeapon != null)
            rangedAttack =currentCharacter.rangedWeapon;
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        base.StartMenu();
        buttons[0].gameObject.SetActive(true);
        if (currentCharacter.rangedWeapon != null) {
            buttons[1].gameObject.SetActive(true);
        }
        if (currentCharacter.GetAllMoves().Count > 0)
        {
            buttons[2].gameObject.SetActive(true);
        }
        //Placeholder condition
        if (0 == 1)
        {
            buttons[3].gameObject.SetActive(true);
        }
        buttons[4].gameObject.SetActive(true);
        buttons[6].gameObject.SetActive(true);
    }
}
