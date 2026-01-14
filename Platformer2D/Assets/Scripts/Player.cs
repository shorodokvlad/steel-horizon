using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : Entity
{
    public Image healthImage;
    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 12;
    private float xInput;
    private bool canJump = true;

    [Header("Fall Settings")]
    [SerializeField] private float fallLimit = -30f;
    [Header("Health Bar")]
    [SerializeField] private Image[] heartImages;         
    [SerializeField] private Sprite[] fullHeartSprites;     
    [SerializeField] private Sprite[] emptyHeartSprites;

    [Header("Energy Bar")]
    [SerializeField] private Image[] energyImages;         
    [SerializeField] private Sprite[] fullEnergySprites;     
    [SerializeField] private Sprite[] emptyEnergySprites;
    [SerializeField] private float energyRegenSpeed;

    private float currentEnergy = 100f;
    private float maxEnergy = 100f;

    private void Start()
    {
        
        maxHealth = 100;
        currentHealth = maxHealth;
        UpdateHearts();
    }
    protected override void Update()

    {
        base.Update();
        HandleInput();

        if (transform.position.y < fallLimit && !isDead)
        {
            Die();
        }
        RegenerateEnergy();
        
        UpdateHearts();
        UpdateEnergyUI();
    }
    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleSuperAttack();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleAttack();
            }
        }
    }
    protected override void Die()
    {
        base.Die();

        PauseMenu.instance.ShowGameOverScreen();
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void HandleSuperAttack()
    {
        if (isGrounded && currentEnergy >= maxEnergy) 
        {
            anim.SetTrigger("superAttack");

            StartCoroutine(StepDrainEnergy());
        }
    }
    private void Jump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthPack") && !isDead && currentHealth < 100)
        {
            Heal(25);
            
            Destroy(collision.gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();    
    }
    private void UpdateHearts()
    {
        if (heartImages == null || heartImages.Length == 0) return;

        
        int fullHeartsCount = Mathf.CeilToInt(currentHealth / 25f);

       
        fullHeartsCount = Mathf.Clamp(fullHeartsCount, 0, 4);

        for (int i = 0; i < heartImages.Length; i++)
        {
            Image heart = heartImages[i];

            if (heart == null) continue;

          
            bool shouldBeFull = i < fullHeartsCount;

          
            if (shouldBeFull)
            {
              
                if (fullHeartSprites != null && fullHeartSprites.Length > i && fullHeartSprites[i] != null)
                    heart.sprite = fullHeartSprites[i];
                else
                    heart.sprite = fullHeartSprites[0]; 
            }
            else
            {
              
                if (emptyHeartSprites != null && emptyHeartSprites.Length > i && emptyHeartSprites[i] != null)
                    heart.sprite = emptyHeartSprites[i];
                else
                    heart.sprite = emptyHeartSprites[0]; 
            }

            heart.color = Color.white;
        }
    }

    private void UpdateEnergyUI()
    {
        if (energyImages == null || energyImages.Length == 0) return;

        for (int i = 0; i < energyImages.Length; i++)
        {
            float threshold = (i + 1) * 25f;

            if (currentEnergy >= threshold)
            {
                energyImages[i].sprite = fullEnergySprites[i];
            }
            else
            {
                energyImages[i].sprite = emptyEnergySprites[i];
            }
        }
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegenSpeed * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        }
    }

    private IEnumerator StepDrainEnergy()
    {
        yield return new WaitForSeconds(1.2f); 

        int steps = 4;
        float drainPerStep = 25f;
        float timeBetweenSteps = 0.2f;

        for (int i = 0; i < steps; i++)
        {
            currentEnergy -= drainPerStep;
            
            if (currentEnergy < 0) currentEnergy = 0;

            UpdateEnergyUI();
            
            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }
    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }
}