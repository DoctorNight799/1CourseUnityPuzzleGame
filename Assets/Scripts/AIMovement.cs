using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField]
    private float speed = 2f;

    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            direction = -direction;
            sr.flipX = !sr.flipX;
        }
    }
}
