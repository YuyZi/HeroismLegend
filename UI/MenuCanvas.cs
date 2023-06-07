using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//访问EventSystem
using UnityEngine.EventSystems;

//实现加载场景后谁定默认选择按钮
public class MenuCanvas : MonoBehaviour
{
    public GameObject newGameButton;
    private void OnEnable() 
    {
        //访问当前激活的事件系统
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

