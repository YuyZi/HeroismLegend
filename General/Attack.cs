using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other) 
    {
        //  ?  等价于 !=null
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}