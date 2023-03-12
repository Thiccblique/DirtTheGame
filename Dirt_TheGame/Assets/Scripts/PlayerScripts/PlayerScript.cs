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
    public int WaitForSeconds = 1;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject projectilePrefab;
    Vector2 lookDirection = new Vector2(1, 0);
    
    private bool projectileShoot = true;
    int currentHealth;
    public int health { get { return currentHealth; } }
    Vector2 movement;

    // Start is called once at the start
    void Start()
    {
        currentState = PlayerState.Walking;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        Vector3 lookDirection = new Vector3(movement.x, movement.y).normalized;
        UpdateAnimationsAndMove();
        PlayerAttacks();
        LookDirection();
      
    }

    // void UpdateAnimationsAndMove is called every fram to animate the Player Movement
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

    // This function Keeps the player idle at the last place they looked
    void LookDirection()
    {
        Vector2 move = new Vector2(movement.x, movement.y);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
    }

    // void PlayerAttacks plays attack animations when it is called
    void PlayerAttacks()
    {
        if (Input.GetButtonDown("attack") && currentState != PlayerState.Attack)
        {
            StartCoroutine(AttackCo());
        }
        
        if (Input.GetButtonDown("shoot") && currentState != PlayerState.Attack && projectileShoot == true)
        {
            StartCoroutine(ShootCo());
            Shoot();
        }
    }

    // void Shoot launches a projectile in the direction the player is facing
    void Shoot()
    {
        MoveCharacter();

        GameObject projectileObject = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        projectile.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg);
    }

    //The two corutins below play the animations they are for when the player calls them
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
        yield return new WaitForSeconds(1);
        currentState = PlayerState.Walking;
    }
    
   // Health Manager for the "PlayerScript" 
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            ReloadCurrentScene();
        }
    }

    // void ReloadCurrentScene rescets the scene when the player heath has reached zero
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
