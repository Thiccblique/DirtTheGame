using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walking, 
    Attack
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
   
    int currentHealth;
    public int health { get { return currentHealth; } }

    Vector2 movement;

    void Start()
    {
        currentState = PlayerState.Walking;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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


    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentState= PlayerState.Attack;
        yield return null;
        animator.SetBool("Attacking", false);
       
        currentState= PlayerState.Walking;
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

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }


    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
