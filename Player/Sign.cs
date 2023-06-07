using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//调用其他输入设备的命名空间
//PS4
using UnityEngine.InputSystem.DualShock;
//XBOX
using UnityEngine.InputSystem.XInput;
public class Sign : MonoBehaviour
{
    private PlayerInputControl playerInput;
    private Animator anim;
    public GameObject signSprite;
    private SpriteRenderer spriteRenderer;
    public Transform playerTrans;
    private IInteractable targetItem;
    private bool canPress;
    private void Awake() 
    {
        //anim = GetComponentInChildren<Animator>();
        anim = signSprite.GetComponent<Animator>();
        //获取并启用
        playerInput = new PlayerInputControl();
        playerInput.Enable();
        spriteRenderer = signSprite.GetComponent<SpriteRenderer>();
    }
    private void OnEnable() 
    {
        //判断输入设备
        InputSystem.onActionChange  += OnActionChange;
        //激活确认按键
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }
    private void OnDisable() 
    {
        canPress = false;
    }
    private void Update() 
    {
        // signSprite.SetActive(canPress);  
        // 开启子物体关闭Renderer    优化逻辑，解决无法获取动画机的问题
        spriteRenderer.enabled = canPress;
        signSprite.transform.localScale = playerTrans.localScale;
    }
    /// <summary>
    /// 切换设备的同时切换按键提示动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj ,InputActionChange actionChange)
    {
        //如果使用了新设备输入  started
        if(actionChange == InputActionChange.ActionStarted)
        {
            //输出当前使用的设备名称    根据打印输出的设备调整swtich 
            //Debug.Log(((InputAction) obj).activeControl.device);
            var d = ((InputAction) obj).activeControl.device;
            switch(d.device)
            {
                case Keyboard:
                    anim.Play("keyboard");
                    break;
                case XInputController:
                    anim.Play("xbox");
                    break;
                default:    
                    Debug.Log("Error:can not find your activecontrol , please add more InputAction to fix it .");
                    break; 
            }
        }
    }
    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if(canPress)
        {
            //Debug.Log("can press :"+canPress);
            //只要实现了接口都能统一调用
            targetItem.TriggerAction();
            //播放音效
            GetComponent<AudioDefination>()?.PlayAduioClip();
            canPress =false;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Interactable"))
        {
            canPress = true;
            //获得物品接口类型
            targetItem = other.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Interactable"))
        {
            canPress = false;
            
        }
    }
}
