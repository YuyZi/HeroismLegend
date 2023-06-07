using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("Chase");
        //currentEnemy.anim.SetBool("walk",false);
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        
        currentEnemy.anim.SetBool("run",true);
    }
    public override void LogicUpdate()
    {   
        //丢失追击目标  变回巡逻
        if(currentEnemy.lostTimeCounter<=0)
        {
            currentEnemy.SwithchState(NPCState.Patrol);
        }
        if(!currentEnemy.physicsCheck.isGround  ||
        (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x<0 )  ||
        (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x>0))
        {   
            //直接转身
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x,1,1);
        }
        // Debug.Log("Walk:"+currentEnemy.anim.GetBool("walk"));
        // Debug.Log("Run:"+currentEnemy.anim.GetBool("run"));
    }


    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        //重置时间
        //currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("run",false);
    }
}
