using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    public float speed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
