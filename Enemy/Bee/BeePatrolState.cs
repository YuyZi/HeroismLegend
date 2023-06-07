using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    //目标位置  以及移动方向
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = currentEnemy.GetNewPoint();
    }
    public override void LogicUpdate()
    {
        //找到玩家  切换追击状态    调用Enemy父类的切换状态
        if(currentEnemy.FindPlayer())
        {
            currentEnemy.SwithchState(NPCState.Chase);
        }
        //计算位置差值 估算是否到达位置  不使用相减，非常难判断
        if(Mathf.Abs(target.x - currentEnemy.transform.position.x)<0.1f
        && Mathf.Abs(target.y -currentEnemy.transform.position.y)<0.1f)
        {
            //等待并获取新的点
            currentEnemy.wait =true;
            target = currentEnemy.GetNewPoint();
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
        if(!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead)
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
    
    }
}
