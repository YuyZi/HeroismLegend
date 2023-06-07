using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu( menuName = "Event/ScenesLoadEventSO")]
public class ScenesLoadEvent_SO : ScriptableObject
{
    //  场景    生成位置        是否播放切换效果
    public UnityAction<GameScene_SO,Vector3,bool>   LoadRequestEvent;
/// <summary>
/// 场景加载请求
/// </summary>
/// <param name="locationToLoad">目标场景</param>
/// <param name="posToGo">角色初始位置</param>
/// <param name="fadeScreen">是否淡入淡出切换</param>
    public void  RaiseLoadRequestEvent(GameScene_SO locationToLoad,
    Vector3 posToGo ,   bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToLoad,posToGo,fadeScreen);
    }
}

