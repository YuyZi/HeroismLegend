using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{   
    private Attack attack;
    private Vector3 target;
    private Vector3 moveDir;
    private bool isAttack;
    public float attackRateCounter = 0;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();
        //避免马上退出状态  反复执行
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;

        currentEnemy.anim.SetBool("chase",true);
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.lostTimeCounter<=0)
        {
            currentEnemy.SwithchState(NPCState.Patrol);
        }
         //计数器 攻击间隔
        attackRateCounter-= Time .deltaTime;
        //计算当前位置与玩家之间的距离
        //玩家中心点在脚底，所以position.y加1.65f
        target = new Vector3(currentEnemy.attacker.position.x,
         currentEnemy.attacker.position.y + 1.65f,0);
        //停止范围 在两者碰撞体Size的一半大小后停止 小于攻击距离后停止 开始攻击
        //Bee Size 1, 1.2  PlayerSize 0.7,2
        if(Mathf.Abs(target.x - currentEnemy.transform.position.x)<= attack.attackRange
        && Mathf.Abs(target.y - currentEnemy.transform.position.y)<= attack.attackRange)
        {
            isAttack = true;
            if(!currentEnemy.isHurt)
            {
                //停止      添加判断条件 否则玩家攻击无法击退
                currentEnemy.rb.velocity = Vector2.zero;
            }

            if(attackRateCounter<=0)
            {
                //播放攻击动画  重置计时
                currentEnemy.anim.SetTrigger("attack");
                attackRateCounter = attack.attackRate;
            }
        }
        else
        {
            //超出攻击范围
            isAttack =false;
        }
        //移动方向
        moveDir = (target - currentEnemy.transform.position).normalized;
        //更改朝向
        if(moveDir.x>0)
        {
            currentEnemy.transform.localScale = new Vector3(-1,1,1);
        }
        if(moveDir.x<0)
        {
            currentEnemy.transform.localScale = new Vector3(1,1,1);
        }
    }

    public override void PhysicsUpdate()
    {
        if(!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
        {
            currentEnemy.rb.velocity = moveDir*currentEnemy.currentSpeed*Time.deltaTime;

        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase",false);
    }
}
