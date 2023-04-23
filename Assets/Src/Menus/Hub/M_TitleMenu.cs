using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;
using UnityEngine;

public class M_TitleMenu : S_MenuSystem
{
    [SerializeField]
    private CH_Func newGame;
    [SerializeField]
    private CH_Func loadGame;
    [SerializeField]
    private CH_Func goToOverworld;
    public R_Save saveData;
    public B_Function loadGameButton;
    [SerializeField]
    private R_Boolean isSave;
    [SerializeField]
    private R_Boolean hasStartedGame;

    private void OnEnable()
    {
        newGame.OnFunctionEvent += NewGame;
        loadGame.OnFunctionEvent += LoadGame;
    }

    private void OnDisable()
    {
        newGame.OnFunctionEvent -= NewGame;
        loadGame.OnFunctionEvent -= LoadGame;
    }

    private void Awake()
    {
        hasStartedGame.boolean = false;
        if (File.Exists("save.RB2"))
            loadGameButton.gameObject.SetActive(true);
        else
            loadGameButton.gameObject.SetActive(false);
    }

    public void NewGame()
    {
        goToOverworld.RaiseEvent();
        isSave.boolean = false;
    }
    public void LoadGame() {

        isSave.boolean = true;
        FileStream fs = new FileStream("save.RB2", FileMode.Open);
        BinaryFormatter bin = new BinaryFormatter();
        s_RPGSave save = (s_RPGSave)bin.Deserialize(fs);
        fs.Close();
        saveData.saveData = save;
        goToOverworld.RaiseEvent();
    }
}
