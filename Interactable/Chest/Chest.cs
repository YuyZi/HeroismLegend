using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable() 
    {
        //是否被开启
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }
    public void TriggerAction()
    {
        if(!isDone)
        {
            //Debug.Log("Chest Open");
            OpenChest();
        }
    }
    private void OpenChest()
    {
        //打开后更改标签使其不提示按键
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
        //this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
