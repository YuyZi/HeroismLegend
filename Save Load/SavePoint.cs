using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEvent_SO saveGameEvent;
    [Header("变量参数")]
    private SpriteRenderer spriteRenderer;
    public Sprite darkSprite;
    public Sprite lightSprite;
    public GameObject lightObject;
    public bool isDone;

    private void Awake() 
    {
        spriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        lightObject.SetActive(isDone);
    }
    private void OnEnable() 
    {
        //是否被开启
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
    }
    public void TriggerAction()
    {
        if(!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = lightSprite;
            lightObject.SetActive(true);
            //TODO:保存数据     呼叫事件        DataManager中监听
            saveGameEvent.RaiseEvent();
            this.gameObject.tag = "Untagged";
        }
    }
}
