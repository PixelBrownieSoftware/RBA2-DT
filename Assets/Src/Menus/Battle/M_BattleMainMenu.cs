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
    [SerializeField]
    private R_Items inventory;
    [SerializeField]
    private R_EnemyGroup currentGroup;

    public R_CharacterList targetList;
    public R_CharacterList players;
    public R_CharacterList opponents;

    public B_Function primaryAttackButton;
    public B_Function secondaryAttackButton;
    public B_Function teritaryAttackButton;
    public B_Function skillsButton;
    public B_Function guardButton;
    public B_Function itemButton;
    public B_Function analyseButton;
    public B_Function passButton;
    public B_Function runButton;

    public s_move analyseMove;
    public s_move guard;
    public s_move pass;

    [SerializeField]
    private CH_Text changeMenu;
    [SerializeField]
    private R_Text menuText;
    [SerializeField]
    private R_Text battleMenuType;

    [SerializeField]
    private CH_Func goToFirst;
    [SerializeField]
    private CH_Func goToSecond;
    [SerializeField]
    private CH_Func goToThird;
    [SerializeField]
    private CH_Func goToSkills;
    [SerializeField]
    private CH_Func goToPass;
    [SerializeField]
    private CH_Func goToGuard;
    [SerializeField]
    private CH_Func performMove;
    [SerializeField]
    private CH_Func goToAnalyse;
    [SerializeField]
    private CH_Func goToItems;
    [SerializeField]
    private CH_Func runFromBattle;

    private void OnEnable()
    {
        goToSkills.OnFunctionEvent += GoToSkills;
        goToFirst.OnFunctionEvent += PrimaryMove;
        goToSecond.OnFunctionEvent += SecondMove;
        goToThird.OnFunctionEvent += ThirdMove;
        goToPass.OnFunctionEvent += PassAction;
        goToGuard.OnFunctionEvent += GuardAction;
        goToAnalyse.OnFunctionEvent += GoToAnalyse;
        runFromBattle.OnFunctionEvent += RunFromBattle;
        goToItems.OnFunctionEvent += GoToItems;
    }

    private void OnDisable()
    {
        goToSkills.OnFunctionEvent -= GoToSkills;
        goToFirst.OnFunctionEvent -= PrimaryMove;
        goToSecond.OnFunctionEvent -= SecondMove;
        goToThird.OnFunctionEvent -= ThirdMove;
        goToPass.OnFunctionEvent -= PassAction;
        goToGuard.OnFunctionEvent -= GuardAction;
        goToAnalyse.OnFunctionEvent -= GoToAnalyse;
        runFromBattle.OnFunctionEvent -= RunFromBattle;
        goToItems.OnFunctionEvent -= GoToItems;
    }

    public void RunFromBattle()
    {
        changeMenu.RaiseEvent("EMPTY");
    }

    public void SelectMove()
    {
        switch (currentMoveRef.move.moveTarg)
        {
            case s_move.MOVE_TARGET.ALLY:
                if (currentMoveRef.move.includeDefeated)
                {
                    targetList.SetCharacters(players.characterListRef);
                }
                else
                {
                    targetList.SetCharacters(players.characterListRef.FindAll(x => x.health > 0));
                }
                break;

            case s_move.MOVE_TARGET.ENEMY:
                if (currentMoveRef.move.includeDefeated)
                {
                    targetList.SetCharacters(opponents.characterListRef);
                }
                else
                {
                    targetList.SetCharacters(opponents.characterListRef.FindAll(x => x.health > 0));
                }
                break;

            case s_move.MOVE_TARGET.ENEMY_ALLY:
                {
                    List<CH_BattleChar> allTargets = new List<CH_BattleChar>();
                    allTargets.AddRange(players.characterListRef);
                    allTargets.AddRange(opponents.characterListRef);
                    targetList.SetCharacters(allTargets);
                }
                break;

            case s_move.MOVE_TARGET.SELF:
                {
                    List<CH_BattleChar> allTargets = new List<CH_BattleChar>();
                    allTargets.Add(currentCharacter);
                    targetList.SetCharacters(allTargets);
                }
                break;
            case s_move.MOVE_TARGET.NONE:

                break;
        }
        switch (currentMoveRef.move.moveTarg)
        {
            default:
                changeMenu.RaiseEvent("TargetMenu");
                break;
            case s_move.MOVE_TARGET.NONE:

                break;
        }

        switch (currentMoveRef.move.moveTargScope)
        {
            case s_move.SCOPE_NUMBER.ALL:
                break;
        }
    }

    public void PrimaryMove()
    {
        currentMoveRef.SetMove(currentCharacterRef.characterRef.characterData.characterDataSource.firstMove);
        SelectMove();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("TargetMenu");
    }
    public void SecondMove()
    {
        currentMoveRef.SetMove(currentCharacterRef.characterRef.characterData.characterDataSource.secondMove);
        SelectMove();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("TargetMenu");
    }
    public void ThirdMove()
    {
        currentMoveRef.SetMove(currentCharacterRef.characterRef.characterData.characterDataSource.thirdMove);
        SelectMove();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("TargetMenu");
    }
    /*
    public void GoToComboSkills()
    {
        print(currentCharacterRef.characterRef.characterData.currentMoves.Count);
        currentMovesRef.SetMoves(currentCharacter.GetAllMoves());
        battleMenuType.text = "Combos";
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("BattleSkillMenu");
    }
    */
    public void GoToAnalyse()
    {
        menuText.text = "CharacterStatus";
        currentMoveRef.SetMove(analyseMove);
        SelectMove();
        changeMenu.RaiseEvent("TargetMenu");
    }
    public void GoToSkills()
    {
        print(currentCharacterRef.characterRef.characterData.currentMoves.Count);
        currentMovesRef.SetMoves(currentCharacter.GetAllMoves());
        battleMenuType.text = "Skills";
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("BattleSkillMenu");
    }
    public void GoToItems()
    {
        battleMenuType.text = "Items";
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("BattleSkillMenu");
    }
    public void GuardAction()
    {
        targetCharacterRef.SetCharacter(currentCharacter);
        currentMoveRef.SetMove(guard);
        performMove.RaiseEvent();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("EMPTY");
    }
    public void PassAction()
    {
        targetCharacterRef.SetCharacter(currentCharacter);
        currentMoveRef.SetMove(pass);
        performMove.RaiseEvent();
        menuText.text = "EMPTY";
        changeMenu.RaiseEvent("EMPTY");
    }

    public override void StartMenu()
    {
        currentCharacter = currentCharacterRef.characterRef;
        secondaryAttackButton.gameObject.SetActive(false);
        teritaryAttackButton.gameObject.SetActive(false);
        skillsButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        base.StartMenu();
        primaryAttackButton.gameObject.SetActive(true);
        guardButton.gameObject.SetActive(true);
        passButton.gameObject.SetActive(true);
        analyseButton.gameObject.SetActive(true);
        if (currentCharacter.GetAllMoves().Count > 0)
        {
            skillsButton.gameObject.SetActive(true);
        }
        if (inventory.inventory.Count > 0)
        {
            itemButton.gameObject.SetActive(true);
        }
        if (currentGroup.enemyGroup.fleeable)
        {
            runButton.gameObject.SetActive(true);
        }
    }
}
