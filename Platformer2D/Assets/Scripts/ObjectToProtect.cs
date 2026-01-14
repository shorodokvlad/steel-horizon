using UnityEngine;

public class ObjectToProtect : Entity
{
    [Header("Extra details")]
    [SerializeField] private Transform player;

    protected override void Update()
    {
        HandleCollision();
        HandleMovement(); 
        HandleAnimations();
        HandleFlips();
    }

    protected override void Die()
    {
        base.Die();

        PauseMenu.instance.ShowGameOverScreen();
    }

    protected override void HandleAnimations() {}
    protected override void HandleFlips()
    {
        if (player.transform.position.x > transform.position.x && facingRight == false)
            Flip();
        else if (player.transform.position.x < transform.position.x && facingRight == true)
            Flip();
    }
}

