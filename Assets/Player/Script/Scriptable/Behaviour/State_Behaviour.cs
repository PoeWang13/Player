using System.Collections.Generic;
using UnityEngine;

public class State_Behaviour : ScriptableObject
{
    [HideInInspector] public List<State_Behaviour> allBehaviours = new List<State_Behaviour>();
}