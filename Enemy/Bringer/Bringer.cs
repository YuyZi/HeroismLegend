using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer : Enemy
{
    protected override void Awake()
    {
        base.Awake();
         patrolState = new BringerPatrolState();
         chaseState = new BringerChaseState();
         //skillState = new BringerSkillState();
    }
    public override bool FindPlayer()
    {
        var obj = Physics2D.BoxCast(transform.position+(Vector3)centerOffset,
        checkSize,0 ,faceDir,checkDistance,attackLayer);
        if(obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }
    // public override void SetSkill(Vector3 target)
    // {
    //     var setPosition = new Vector3 (target.x,target.y+2.5f,target.z);
    //     Instantiate(skillObject,setPosition,Quaternion.identity);

    // }
    public override void Move()
    {
        
    }
}
