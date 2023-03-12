using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dameger : MonoBehaviour
{
    private bool flashActive;
    [SerializeField]
    private float flashLength = 0f;
    private float flashCounter = 0f;
    private SpriteRenderer enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // If Player enters this zone, it deducts 1 health point from them
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript controller = other.GetComponent<PlayerScript>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
