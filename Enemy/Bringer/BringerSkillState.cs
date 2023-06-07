// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BringerSkillState : BaseState
// {
//     private Vector3 target;
//     private bool isSkill;
//     private float waitTimeCounter=3f;
//     public override void OnEnter(Enemy enemy)
//     {
//         currentEnemy = enemy;
//         currentEnemy.currentSpeed = 0;
//         currentEnemy.anim.SetBool("walk",false);
//         currentEnemy.anim.SetTrigger("skill");
//         currentEnemy.anim.SetBool("cast",true);
//         //找到玩家位置  仅开始时获取一次        y+2.5f
//         target = new Vector3(currentEnemy.attacker.position.x,currentEnemy.attacker.position.y,0);
//         Debug.Log(target);
//         //释放期间无敌
//         currentEnemy.GetComponent<Character>().invulnerable = true;
//         currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
//         currentEnemy.SetSkill(target);
//     }
    
//     public override void LogicUpdate()
//     {
//         waitTimeCounter-=Time.deltaTime;
//         if(waitTimeCounter<=0)
//         {   
//             currentEnemy.anim.SetBool("cast",false);
//             isSkill = false;
//             //放完技能开始追击
//             currentEnemy.SwithchState(NPCState.Chase);
//         }
//          //避免在Update中马上退出无敌状态 
//          currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;
//     }

//     public override void PhysicsUpdate()
//     {
//     }

//     public override void OnExit()
//     {
//         currentEnemy.GetComponent<Character>().invulnerable = false;
//     }
// }
