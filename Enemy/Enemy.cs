using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]
public class Enemy : MonoBehaviour
{
   [HideInInspector]public Rigidbody2D rb;
   [HideInInspector]public Animator anim;
   [HideInInspector]public PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed;
    [HideInInspector]public float currentSpeed;
    [HideInInspector]public Vector3 faceDir;
    public float hurtForce;
    public Transform attacker;
    public Vector3 spawnPoint;

    [Header("玩家检测")]
    //BoxCast 偏移值
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    [Header("计时器")]
    [HideInInspector]public float waitTimeCounter;
    public float waitTime;
    [HideInInspector]public bool wait;
    [HideInInspector]public float lostTimeCounter;
    public float lostTime;

     [Header("状态")]
    public bool isHurt;
    public bool isDead;
    //有限状态机拓展
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState skillState;
    protected BaseState currentState;
    [Header("技能")]
    public GameObject skillObject;
    protected virtual void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;

        spawnPoint = transform.position;
        //waitTimeCounter= waitTime;
    }
    private void OnEnable() 
    {
        currentState= patrolState;
        //传入当前对象
        currentState.OnEnter(this);
    }
    private void Update() 
    {
        //实时获取人物朝向
        faceDir = new Vector3(-transform.localScale.x,0,0);

        currentState.LogicUpdate();
        TimeCounter();
    }
    private void FixedUpdate() 
    {   
        if(!isHurt&&!isDead &&!wait)
            Move();
        currentState.PhysicsUpdate();
    }
    private void OnDisable() 
    {
        currentState.OnExit();
    }
    /// <summary>
    /// 移动    虚函数 供子类重写  
    /// </summary>
    public virtual void Move()
    {   
        //获取Layer 层级 baselayer 为0      如果播放premove则不移动
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("PreMove") &&
        !anim.GetCurrentAnimatorStateInfo(0).IsName("HideRecover")  )
        //敌人图片默认向左 即将Vector3设置为-localscale.x 即可
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime,rb.velocity.y);
    }
    /// <summary>
    /// 计时器
    /// </summary>
    public void TimeCounter()
    {   
        //巡逻撞墙
        if(wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if(waitTimeCounter<=0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                //转身
                 transform.localScale = new Vector3(faceDir.x,1,1);
            }
        }
        //丢失目标
        if(!FindPlayer() && lostTimeCounter>0)
        {
            lostTimeCounter-=Time.deltaTime;
        }
        else if(FindPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }
    //找到玩家后进入对应的状态脚本中
    //如目前是PatoralState 则传入参数到SwitchState
    public virtual bool FindPlayer()
    {
        //返回值为Raycast 2D        
        // 中心点， 尺寸，角度，  方向， 距离 ，层级
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset,
        checkSize,0 ,faceDir,checkDistance,attackLayer);
    }
    //切换状态时调用 并传入值       根据枚举中的状态切换
    public void SwithchState(NPCState state)
    {
        //同switch case
        //新语法  用于简单变量切换
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            //默认返回null
            _=> null
        };
        //退出上一个状态 
        //执行新状态
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }
    //仅在蜜蜂中重写
    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }
    //仅骷髅中重写
    // public virtual void SetSkill(Vector3 target)
    // {
        
    // }
    #region  事件执行方法
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="attacker"></param>
    public void OnTakeDamage(Transform attackerTrans)
    {
        //设为攻击对象
        attacker = attackerTrans;
        //转身
        //位置>0 为右侧  反之左侧
        if(attackerTrans.position.x  -transform.position.x >0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        if(attackerTrans.position.x  -transform.position.x <0)
            transform.localScale = new Vector3(1,1,1);
        //受击后被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        //                  找到方向  normalized 获取值
        Vector2 dir = new Vector2(transform.position.x - attackerTrans.position.x,0).normalized;
       //被攻击后取消力 并被击退 防止追击状态抵消
        rb.velocity = new Vector2(0,rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }
    IEnumerator OnHurt(Vector2 dir)
    {        
        //添加方向力
        rb.AddForce(dir*hurtForce,ForceMode2D.Impulse);
        //等待0.5f
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    public void OnDie()
    {
        gameObject.layer =2;
        anim.SetBool("dead",true);
        isDead = true;
    }
    //Animation Events
    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion
    //绘制检测范围
    public virtual void OnDrawGizmosSelected() 
    {
        //检测距离
        Gizmos.DrawWireSphere(transform.position+(Vector3)centerOffset
        + new Vector3(checkDistance*(-transform.localScale.x),0)
        ,0.2f);
        Gizmos.color = Color.red;
        //检测大小
        Gizmos.DrawWireCube(transform.position+(Vector3)centerOffset,
        checkSize);
    }
}
