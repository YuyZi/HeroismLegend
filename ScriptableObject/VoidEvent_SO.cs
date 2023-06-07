using UnityEngine;
using UnityEngine.Events;



[CreateAssetMenu(menuName ="Event/VoidEventSO")]
//没有参数的事件    
public class VoidEvent_SO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
