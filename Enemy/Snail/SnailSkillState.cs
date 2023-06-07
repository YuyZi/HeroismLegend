using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSkillState : BaseState
{
    //避免攻击被判定为发现玩家 需要将玩家攻击子物体改为Default
        public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //设置速度为0
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        //取消行走 开始缩壳
        currentEnemy.anim.SetBool("walk",false);
        currentEnemy.anim.SetBool("hide",true);
        currentEnemy.anim.SetTrigger("skill");


        //缩壳后无敌
        currentEnemy.GetComponent<Character>().invulnerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }
    public override void LogicUpdate()
    {
        //丢失目标  变回巡逻
        if(currentEnemy.lostTimeCounter<=0)
        {
            currentEnemy.SwithchState(NPCState.Patrol);
        }
        //避免在Update中马上退出无敌状态 
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide",false);
         currentEnemy.GetComponent<Character>().invulnerable = false;
    }
}
