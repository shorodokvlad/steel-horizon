using System;
using UnityEngine;

public class Enemy : Entity
{
    protected bool playerDetected;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }
    protected override void HandleAttack()
    {
        if (playerDetected)
            anim.SetTrigger("attack");
    }
    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y); 
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  
    }

    protected override void HandleCollision()
    {
        base.HandleCollision(); 
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }

    protected override void Die()
    {
        base.Die();

        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.UpdateKillCount();
        }
    }
}