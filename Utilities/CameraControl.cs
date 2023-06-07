using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("监听")]
    public VoidEvent_SO afterSceneLoadEvent;
   private CinemachineConfiner2D confiner2D;
   public CinemachineImpulseSource impulseSource;
    public VoidEvent_SO cameraShakeEvent;
   private void Awake() 
   {
    confiner2D = GetComponent<CinemachineConfiner2D>();
   }
    private void OnEnable() 
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
    }
    private void OnDisable() 
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
    }
   private void OnCameraShakeEvent()
   {
        //播放摄像机震动
        impulseSource.GenerateImpulse();
   }
   public void OnAfterSceneLoadEvent()
   {
        //获取新的摄像机边界
        GetNewCamaeraBrounds();
   }
    //场景切换后更改
//    private void Start() 
//    {
//     GetNewCamaeraBrounds();
//    }
   //获取所有场景中的Brounds边界
   private void GetNewCamaeraBrounds()
   {
        var obj = GameObject.FindWithTag("Bounds");
        if(obj ==null)
            return;
        //print("obj:"+obj);
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        //在切换场景后需要使用Invalidate Cache清理之前场景的缓存
        confiner2D.InvalidateCache();
   }
}
