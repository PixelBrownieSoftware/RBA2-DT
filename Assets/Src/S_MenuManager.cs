using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class S_MenuManager : MonoBehaviour
{
    public S_MenuSystem[] _menus;
    [SerializeField]
    private CH_Text switchMenuFunction;
    [SerializeField]
    private CH_Func backMenuFucntion;
    private S_MenuSystem m_currentMenu;

    private Stack<S_MenuSystem> menuHistory = new Stack<S_MenuSystem>();

    private void OnEnable()
    {
        backMenuFucntion.OnFunctionEvent += BackFunction;
        switchMenuFunction.OnTextEventRaised += SwitchMenu;
    }

    private void OnDisable()
    {
        backMenuFucntion.OnFunctionEvent -= BackFunction;
        switchMenuFunction.OnTextEventRaised -= SwitchMenu;
    }

    private void Awake()
    {
        _menus = transform.GetComponentsInChildren<S_MenuSystem>();
        foreach (var m in _menus)
            m.gameObject.SetActive(false);
    }

    public void BackFunction() {
        menuHistory.Pop();
        S_MenuSystem ind = menuHistory.Peek();
        SwitchMenu(ind, true);
    }

    public void SwitchMenu(S_MenuSystem menu, bool isBack) {
        if (m_currentMenu != null)
            m_currentMenu.gameObject.SetActive(false);
        m_currentMenu = menu;
        if(!isBack)
            menuHistory.Push(m_currentMenu);
        m_currentMenu.gameObject.SetActive(true);
        m_currentMenu.StartMenu();
    }

    public void SwitchMenu(string menuName)
    {
        for (int i =0; i < _menus.Length; i++) {
            var m = _menus[i];
            if (m.name == menuName)
            {
                SwitchMenu(m, false);
                break;
            }
        }
    }

}
