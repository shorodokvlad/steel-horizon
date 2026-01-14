using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    private Collider2D col;
    protected Entity_VFX entityVfx;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth = 100;
    protected bool isDead;
    
    [Header("Attack Details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected  LayerMask whatIsTarget;

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] protected bool isGrounded; 
    [SerializeField] private LayerMask whatIsGround;

    [Header("Facing Direction Details")]
    [SerializeField] protected int facingDir = 1;
    [SerializeField] protected bool facingRight = true;
    protected bool canMove = true;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>(); 
        col = GetComponent<Collider2D>();

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleMovement(); 
        HandleAnimations();
        HandleFlips();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach(Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        } 
    }

    public virtual void EnableMovement(bool enable)
    {
         canMove = enable;
    }

    protected virtual void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);     
        anim.SetBool("isGrounded", isGrounded);
    }

    protected virtual void HandleMovement() {}

    protected virtual void HandleFlips()
    {
        if (rb.linearVelocity.x > 0 && facingRight == false)
            Flip(); 
        else if (rb.linearVelocity.x < 0 && facingRight == true) 
            Flip();
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }
    protected virtual void HandleAttack()
    {
        if (isGrounded) 
            anim.SetTrigger("attack");
    }

    protected virtual void TakeDamage()
    {
        if (isDead)
            return;

        currentHealth -= 25;
        if (currentHealth <= 0)
        {
            Die();
        }

        entityVfx.PlayOnDamageVfx();
    }

    protected virtual void Die()
{
    if (isDead)
        return;

    isDead = true;

    // Stop logic
    EnableMovement(false);

    // Stop physics movement
    rb.linearVelocity = Vector2.zero;
    rb.bodyType = RigidbodyType2D.Dynamic;

    // Disable collisions
    col.enabled = false;

    // Play death animation
    //anim.SetTrigger("dead");

    // Optional: small knockback / fall effect
    //rb.gravityScale = 2f;
    //rb.AddForce(Vector2.up * 4f, ForceMode2D.Impulse);

    // Destroy after animation
    Destroy(gameObject, 2.5f);
}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));  
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius); 
    }
}

