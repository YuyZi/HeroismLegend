using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [HideInInspector]public PhysicsCheck physicsCheck;
    [HideInInspector]public PlayerController playerController;
    private void Awake() 
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update() 
    {
        SwitchAnimation();
    }
    public void SwitchAnimation()
    {
        //取绝对值  
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY",(rb.velocity.y));
        anim.SetBool("isGround",physicsCheck.isGround);
        anim.SetBool("isCrouch",playerController.isCrouch);
        anim.SetBool("isDead",playerController.isDead);
        anim.SetBool("isAttack",playerController.isAttack);
        anim.SetBool("onWall",physicsCheck.onWall);
        anim.SetBool("isSlide",playerController.isSlide);
    }
    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }
    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
}

