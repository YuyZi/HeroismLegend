using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("事件监听")]
    public ScenesLoadEvent_SO sceneLoadEvent;
    public VoidEvent_SO afterSceneLoadEvent;
    public VoidEvent_SO loadDataEvent;
    public VoidEvent_SO backToMenuEvent;
    //获取脚本组件后可以获取所有的公开属性对象
    [HideInInspector]public Character character;
    [HideInInspector]public PhysicsCheck physicsCheck;
    [HideInInspector]public PlayerAnimation playerAnimation;
    public PlayerInputControl inputControl;
    [HideInInspector]public Vector2 inputDriection;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private BoxCollider2D boxcoll;
    [Header("基本参数")]
    public float speed;
    private float runSpeed ;
    private float walkSpeed => speed / 3f;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;
    [Header("物理材质")]
    public PhysicsMaterial2D normalMaterial;
    public PhysicsMaterial2D wallMaterial;
    private void Awake() 
    {
        runSpeed = speed;
        character = GetComponent<Character>();
        coll = GetComponent<CapsuleCollider2D>();
        boxcoll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        //实例化
        inputControl = new PlayerInputControl();
        originalOffset = coll.offset;
        originalSize = coll.size;
        // started表示按下   perform表示持续  cancled表示取消
        //跳跃                  +=注册事件        
        inputControl.GamePlay.Jump.started += Jump;
        #region  走路与跑动的速度切换
        
        inputControl.GamePlay.WalkBotton.performed += ctx =>
        {
            if(physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };
         inputControl.GamePlay.WalkBotton.canceled +=ctx =>
         {
            if(physicsCheck.isGround)
            {
                speed = runSpeed;
            }
         };
        #endregion
        //攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;
        //滑铲
        inputControl.GamePlay.Slide.started += Slide;
        inputControl.Enable();
    }

    private void OnEnable() 
    {
        //inputControl.Enable();
        sceneLoadEvent.LoadRequestEvent  += OnLoadSceneEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }
    private void OnDisable() 
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent  -= OnLoadSceneEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }
    private void Update() 
    {
       inputDriection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        //print(inputControl.GamePlay.Move.ReadValue<Vector2>());
        CheckState();
    }
    private void FixedUpdate() 
    {
        if(!isHurt&& !isAttack )
            Move();
    }
    //场景加载
    private void OnLoadSceneEvent(GameScene_SO target,Vector3 position,bool FadeIn)
    {
        //加载时停止角色控制
        inputControl.GamePlay.Disable();
    }
        //读取游戏进度
    private void OnLoadDataEvent()
    {
        //取消死亡状态
        isDead =false;
    }
    //加载结束
    private void OnAfterSceneLoadEvent()
    {
        //加载完毕后恢复角色控制
        inputControl.GamePlay.Enable();
    }



    //void  代表没有返回值
    public void Move()
    {
        if(!isCrouch && !wallJump) 
        rb.velocity =  new Vector2(inputDriection.x*speed*Time.deltaTime,rb.velocity.y);
        int faceDirection = (int)transform.localScale.x;
        if(inputDriection.x>0)
        {
            faceDirection=1;
        }
        else if(inputDriection.x<0)
        {
            faceDirection=-1;
        }
        //人物翻转          或使用SpriteRenderer中的Flip        注意 使用localScale需要确保人物中心轴对称
        transform.localScale = new Vector3 (faceDirection,1,1);
        //下蹲  且在地面
        isCrouch = inputDriection.y<-0.5f && physicsCheck.isGround;
        if(isCrouch)
        {
            //修改碰撞体大小
            coll.offset = new Vector2(-0.1f,0.85f);
            coll.size = new Vector2(0.7f,1.7f);
        }
        else
        {
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        //空中无法攻击
        if(!physicsCheck.isGround)  
            return;
        playerAnimation.PlayAttack();
        isAttack = true;
        //设置计数会导致计数增长快而无法切换
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        var jumpClip = transform.GetChild(2).gameObject;
        if(physicsCheck.isGround)
        {
            jumpClip.GetComponent<AudioDefination>().PlayAduioClip();
            //力的模式  瞬时力与普通力  这里使用瞬时力
            rb.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
            //打断滑铲携程
            isSlide = false;
            StopAllCoroutines();
            //gameObject.layer = LayerMask.NameToLayer("Player");
            /*  停止指定携程
            Coroutine slideIEnumerator =  StartCoroutine(TriggerSlide(targetPos));
            StopCoroutine(slideIEnumerator)
            */
        }
        else if(physicsCheck.onWall)
        {
            jumpClip.GetComponent<AudioDefination>().PlayAduioClip();
            //朝面朝方向或键盘输入的反方向      如贴左墙则往右上方跳跃 需要一个x方向的力
            rb.AddForce(new Vector2(-inputDriection.x,2.5f)*wallJumpForce,ForceMode2D.Impulse);
            //防止与移动冲突无法跳跃
            wallJump = true;
        }

    }
    private void Slide(InputAction.CallbackContext obj)
    {
        var SlideClip = transform.GetChild(3).gameObject;
        if(!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;
            //播放音乐
            SlideClip.GetComponent<AudioDefination>().PlayAduioClip();
            //切换碰撞体
            coll.enabled = false;
            boxcoll.enabled =true;
            var targetPos = new Vector3(transform.position.x + slideDistance*transform.localScale.x ,transform.position.y);
            //更改层级避免碰撞以及受伤 在CheckState中实现
            //gameObject.layer = 6;
            //gameObject.layer = LayerMask.NameToLayer("Enemy");
            //Debug.Log(gameObject.layer);
           StartCoroutine(TriggerSlide(targetPos));
            //Debug.Log(targetPos);
            //减少体力条
            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do{
            yield return null;
            //当移动到边缘时携程停止  防止冲出平台
            if(!physicsCheck.isGround)
                break;
            //滑铲时撞墙
            if(physicsCheck.touchLeftWall && transform.lossyScale.x < 0f || 
            physicsCheck.touchRightWall && transform.lossyScale.x > 0f)
                break;
            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed , transform.position.y));
            //Debug.Log(Mathf.Abs(target.x - transform.position.x));
        }while(Mathf.Abs(target.x - transform.position.x) > 0.2f);
        //持续不断移动到目标点
        isSlide = false;
        //切换碰撞体
        coll.enabled = true;
        boxcoll.enabled =false;
        //gameObject.layer = LayerMask.NameToLayer("Player");
        //Debug.Log(gameObject.layer);
    }
    #region  UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        //受伤后停下
        rb.velocity = Vector2.zero;
        //受伤方向      人物左 为-  人物右 为+          归一化
        Vector2 dir = new Vector2((transform.position.x-attacker.position.x),0).normalized;
        rb.AddForce(dir * hurtForce,ForceMode2D.Impulse);
    }
    public void PlayerDead()
    {
        isDead = true;
        //取消输入操作
        inputControl.GamePlay.Disable();
        var DeadClip = transform.GetChild(4).gameObject;
        DeadClip.GetComponent<AudioDefination>().PlayAduioClip();
    }
    #endregion
    public void CheckState()
    {   
        //死亡避免敌人再次攻击
         if (isDead || isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else 
            gameObject.layer = LayerMask.NameToLayer("Player");
        //根据状态选择材质      在地上用normal  否则用wall
        coll.sharedMaterial = physicsCheck.isGround ? normalMaterial : wallMaterial;
        //贴墙滑行速度减缓
        if(physicsCheck.onWall)
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/2f);
        }
        else
             rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y);
        //贴墙跳到最高点时恢复移动
        if(wallJump && rb.velocity.y<0f)
        {
            wallJump = false;
        }
    }
}


