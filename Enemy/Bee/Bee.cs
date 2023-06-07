using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("移动范围")]
    public float patrolRadius;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState(); 
        chaseState = new  BeeChaseState();
    }
    //重写
    public override bool FindPlayer()
    {
        //中心点 半径 图层
       var obj =  Physics2D.OverlapCircle(transform.position,checkDistance,attackLayer);
        if(obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }
    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,checkDistance);
        Gizmos.color =Color.blue;
        Gizmos.DrawWireSphere(spawnPoint,patrolRadius);
    }
    //获取随机巡逻点
    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius,patrolRadius);
        var targetY = Random.Range(-patrolRadius,patrolRadius);
        return spawnPoint + new Vector3 (targetX,targetY);
    }
    public override void Move()
    {
        
    }
}
