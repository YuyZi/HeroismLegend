using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/FadeEvent_SO")]
public class FadeEvent_SO : ScriptableObject
{
    //场景切换中使用
    public UnityAction<Color,float,bool>    OnEventRaised;
    /// <summary>
    /// 逐渐变黑
    /// </summary>
    /// <param name="duration">持续时间</param>
   public void FadeIn(float duration)
   {
        RaiseEvent(Color.black,duration,true);
   }
    /// <summary>
    /// 逐渐透明
    /// </summary>
    /// <param name="duration">持续时间</param>
   public void FadeOut(float duration)
   {
        RaiseEvent(Color.clear,duration,false);
   }
   public void RaiseEvent(Color target ,float duration ,bool fadeIn)
   {
        OnEventRaised?.Invoke(target,duration,fadeIn);
   }
}
