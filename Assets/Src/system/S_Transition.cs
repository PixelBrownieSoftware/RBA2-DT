using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "TransitionManager", menuName = "Transition Manager")]
public class S_Transition : ScriptableObject
{
    public string _currentLocation;
    public string _prevLocation;
    public UnityAction onTransition;
}
