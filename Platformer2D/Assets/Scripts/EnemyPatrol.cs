using UnityEngine;

public class EnemyPatrol : Enemy
{
    [Header("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Chase Behavior")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float verticalRange = 2f;

    private Transform target;
    private bool isChasing;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private bool isIdling;


    protected override void Update()
{
    if (isDead)
        return;
        
    base.Update();
    DetectPlayer();
    HandleAttack();
}
    protected override void HandleAttack()
    {
        if (playerDetected && canMove)
        {
            EnableMovement(false);
            anim.SetTrigger("attack");
        }
    }
    
    protected override void HandleMovement()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        
        if (isIdling)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                isIdling = false;
                idleTimer = 0;
                Flip();
            }
            return;
        }

        if (isChasing && target != null)
        {
            float dir = target.position.x > transform.position.x ? 1f : -1f;

            rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);

            if (dir > 0 && !facingRight) Flip();
            else if (dir < 0 && facingRight) Flip();

            return;
        }
        
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }


    protected override void HandleFlips()
    {
        if (isIdling)
            return;
        
        if (facingRight && transform.position.x >= rightEdge.position.x)
        {
            StartIdle();
        }
        else if (!facingRight && transform.position.x <= leftEdge.position.x)
        {
            StartIdle();
        }
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, chaseRange, whatIsTarget);

        if (hit != null)
        {
            float targetX = hit.transform.position.x;
            float targetY = hit.transform.position.y;
            float myY = transform.position.y;

            // 1. Calculate the vertical difference
            float yDiff = Mathf.Abs(targetY - myY);

            // 2. Check if player is within patrol edges AND on the same height level
            if (targetX >= leftEdge.position.x && targetX <= rightEdge.position.x && yDiff <= verticalRange)
            {
                target = hit.transform;
                isChasing = true;
                return;
            }
        }

        // Stop chasing if player is gone, out of bounds, or on a different level
        isChasing = false;
        target = null;
    }

    private void StartIdle()
    {
        isIdling = true;
        idleTimer = 0;
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

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
