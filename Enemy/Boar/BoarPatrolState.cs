using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //可以通过访问Enemy更改数值等操作
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        //Debug.Log("Enter");
    }
    public override void LogicUpdate()
    {
        //找到玩家  切换追击状态    
        if(currentEnemy.FindPlayer())
        {
            currentEnemy.SwithchState(NPCState.Chase);
        }
        //撞墙转向 以及悬崖勒马          同时判断面朝方向是否相同  否则会反复横跳
        else if(!currentEnemy.physicsCheck.isGround  ||
        (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x<0 )  
        ||(currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x>0))
        {   
            currentEnemy.wait = true;
            //更改动画
            currentEnemy.anim.SetBool("walk",false);
            //transform.localScale = new Vector3(faceDir.x,1,1);
        }
        else
        {   //让野猪可以播放行走的动画
            currentEnemy.anim.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        //退出期间执行Update又更改状态
       currentEnemy.anim.SetBool("walk",false);
       //Debug.Log("Exit:Walk"+currentEnemy.anim.GetBool("walk"));
    }
}
