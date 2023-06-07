using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private  CapsuleCollider2D coll;
    private Rigidbody2D rb;
    private PlayerController playerController;
    [Header("检测参数")]
    //是否手动设置偏移
    public bool manual;
    //玩家独有
    public bool isPlayer;
    //位移差值 
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    //检测范围
    public float checkRaduis;
    public LayerMask groundLayer;
    [Header("状态")]
    public bool isGround;
    public bool onWall;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake() 
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //如果非手动设置
        if(!manual)
        {   
            //coll.bounds 即为 设定的碰撞体大小 
            rightOffset = new Vector2((coll.bounds.size.x+coll.offset.x)/2,coll.bounds.size.y/2); 
            leftOffset = new Vector2(-rightOffset.x,rightOffset.y); 
        }
        if(isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }
    private void Update() 
    {
        Check();
    }
    public void Check()
    {   
        // * 上localscale实现人物转向检测点也转向
        //检测地面              中心点          范围            层级
        if(onWall)
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + 
                new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y),
                    checkRaduis, groundLayer);
        else
            //正常移动情况未设置bottomoffset.y 所以直接赋值0在贴墙时更改地面监测点
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + 
                new Vector2(bottomOffset.x * transform.localScale.x, 0),
                    checkRaduis, groundLayer);
        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + 
            new Vector2(leftOffset.x, leftOffset.y), 
                checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + 
            new Vector2(rightOffset.x, rightOffset.y), 
                checkRaduis, groundLayer);

        //在墙壁上      跳跃后按住方向键贴墙滑行 跳至最高点rb.y0时开始下落
        //bug 使用!isGround 在按住方向键时无法起跳
        if(isPlayer)
        {
            onWall = ( touchLeftWall && playerController.inputDriection.x<0f || 
            touchRightWall && playerController.inputDriection.x>0f )&& rb.velocity.y<0f;   
        }
    }
    //选中时进行绘制
    private void OnDrawGizmosSelected() 
    {
        //中心点，检测范围
        //绘制时 为贴墙时的检测情况 bottomoffset.y !=0;
        Gizmos.DrawWireSphere((Vector2)transform.position + 
            new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + 
            new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + 
            new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }
}
