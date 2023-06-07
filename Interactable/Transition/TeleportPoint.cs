using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public ScenesLoadEvent_SO loadEventSO;

    public GameScene_SO sceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log ("传送");
        //启动加载事件 加载场景         呼叫广播
        loadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGo,true);
    }
}
