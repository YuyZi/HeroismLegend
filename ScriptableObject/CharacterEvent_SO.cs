using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName =("Event/CharacterEventSO"))]
public class CharacterEvent_SO : ScriptableObject
{
    //事件订阅
    public UnityAction<Character> OnEventRaised;
    //启用事件就传入自身属性
    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke(character);
    }
}
 