using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    
    public override void LogicUpdate()
    {
        //找到玩家  切换追击状态    调用Enemy父类的切换状态
        if(currentEnemy.FindPlayer())
        {
            //先进入技能状态再进入追击
            //currentEnemy.SwithchState(NPCState.Skill);
            currentEnemy.SwithchState(NPCState.Chase);
            //Debug.Log("find");
        }
        //撞墙转向 以及悬崖勒马          同时判断面朝方向是否相同  否则会反复横跳
        if(!currentEnemy.physicsCheck.isGround  ||
        (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x<0 )  ||
        (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x>0))
        {   
            currentEnemy.wait = true;
            //更改动画
            currentEnemy.anim.SetBool("walk",false);
            //transform.localScale = new Vector3(faceDir.x,1,1);
        }
        else
        {   //让野猪可以播放行走的动画
            currentEnemy.anim.SetBool("walk", true);
            //Debug.Log("bool"+currentEnemy.anim.GetBool("walk"));
        }
    }

    public override void PhysicsUpdate()
    {
         if(!currentEnemy.isHurt && !currentEnemy.isDead )
        {
            currentEnemy.rb.velocity = currentEnemy.faceDir*currentEnemy.currentSpeed*Time.deltaTime;
        }
        else
        {
            currentEnemy.rb.velocity = Vector2.zero;
        }
    }

    public override void OnExit()
    {
       currentEnemy.anim.SetBool("walk",false);

       //Debug.Log("bool"+currentEnemy.anim.GetBool("walk"));
    }
}
