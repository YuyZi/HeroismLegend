using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{   
    public  PlayerStateBar  playerStateBar;
    [Header("事件监听")]
    public CharacterEvent_SO healthEvent;
    public ScenesLoadEvent_SO unloadSceneEvent;
    public VoidEvent_SO loadDataEvent;
    public VoidEvent_SO gameOverEvent;
    public VoidEvent_SO backToMenuEvent;
    [Header("组件")]
    public GameObject GameOverPanel;
    public GameObject restartBtn;
    //订阅与取消  可以同时多个代码订阅同一个事件
    //注册事件 与取消注册
    private void OnEnable() 
    {
        healthEvent.OnEventRaised += OnHealthEvent ;
        unloadSceneEvent.LoadRequestEvent += OnUnLoadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable() 
    {
        healthEvent.OnEventRaised -= OnHealthEvent ;
        unloadSceneEvent.LoadRequestEvent -= OnUnLoadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }
    private void OnGameOverEvent()
    {
        GameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }
    private void OnLoadDataEvent()
    {
        GameOverPanel.SetActive(false);

    }
    private void OnUnLoadSceneEvent(GameScene_SO sceneToLoad,Vector3 position,bool FadeIn)
    {
        //主菜单时  取消显示角色UI
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
            playerStateBar.gameObject.SetActive(!isMenu);
        
    }
    private void OnHealthEvent(Character character)
    {
        //FIXME:数值计算BUG导致UI无法正常显示  整数相除的bug
        //获取百分比传递到PlayerStateBar
       float persentage = (character.currentHealth / character.maxHealth);
       //Debug.Log("persentage："+persentage);
       //Debug.Log("？？？"+character.currentHealth / character.maxHealth );
       playerStateBar.OnHealthChange(persentage);
       playerStateBar.OnPowerChange(character);
    }

}
