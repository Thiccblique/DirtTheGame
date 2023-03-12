using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    private Animator myAnim;
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        // tells the Enemy that they would chase whatever has the "PlayerScript" attached
        target = FindObjectOfType<PlayerScript>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player enters the range of the enemy, they will start to chase.
        if (Vector3.Distance(target.position, transform.position) <= range)
        {
            FollowPlayer();
        }
    }

    public void FollowPlayer()
    {
        // The animation Played depending on the direction the enemy is faceing
        myAnim.SetBool("isMoving", true);
        myAnim.SetFloat("moveX", (target.position.x - transform.position.x));
        myAnim.SetFloat("moveY", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // when the enemy enters something taged "MyWeapon" they recive knockback and are telleported a bit back
        if (other.tag == "MyWeapon")
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);
        }
    }
   
}
