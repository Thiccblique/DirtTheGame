using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    Walking, 
    Attack,
    Shoot
}

public class PlayerScript : MonoBehaviour
{
    public PlayerState currentState;
    public float moveSpeed = 5.0f;
    public int maxHealth = 10;
    public int currHealth;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject projectilePrefab;
    Vector2 lookDirection = new Vector2(1, 0);

    private bool flashActive;
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer enemySprite;

    


    int currentHealth;
    public int health { get { return currentHealth; } }
    
    Vector2 movement;

    void Start()
    {
        currentState = PlayerState.Walking;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
       
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FlashStart()
    {
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationsAndMove();

        if(Input.GetButtonDown("attack") && currentState != PlayerState.Attack)
        {
            StartCoroutine(AttackCo());
        }

        if (Input.GetButtonDown("shoot") && currentState != PlayerState.Attack)
        {
            StartCoroutine(ShootCo());
        }

        if (currentHealth <= 0)
        {
            ReloadCurrentScene();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Launch();
        }
        Vector2 move = new Vector2(movement.x, movement.y);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
       
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);


    }



    public void HurtEnemy(int damageToGive)
    {
        currentHealth -= damageToGive;
        flashActive = true;
        flashCounter = flashLength;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentState= PlayerState.Attack;
        yield return null;
        animator.SetBool("Attacking", false);
       
        currentState= PlayerState.Walking;
       
        
    }

    private IEnumerator ShootCo()
    {
        animator.SetBool("Shooting", true);
        currentState = PlayerState.Shoot;
        yield return null;
        animator.SetBool("Shooting", false);

        currentState = PlayerState.Walking;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    void UpdateAnimationsAndMove()
    {
        MoveCharacter();

        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }


    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}
