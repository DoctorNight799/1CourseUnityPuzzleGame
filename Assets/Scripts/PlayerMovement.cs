using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private bool dead = false;
    private bool win = false;
    private bool isOnWall = false;

    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject continuePanel;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float groundCheckRad = 0.05f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float divideJump = 2f;

    [SerializeField]
    private float fallSpeed = 2f;
    [SerializeField]
    private float jumpMultiplier = 2;

    private float jumpCounter = 0, jumpTime = 0.25f;
    private bool inJump = false;

    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dead = false;
        win = false;
    }

    void Update()
    {
        Move();
        Jump();
        DeadCheck();
        NextLevel();
    }

    void DeadCheck()
    {
        if (dead)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gameOverPanel.SetActive(false);
        }
    }

    void NextLevel()
    {
        if (win)
        {
            Time.timeScale = 0;
            continuePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            continuePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            direction = -direction;
            transform.Rotate(0, 180, 0);
            isOnWall = true;
        }

        if (collision.gameObject.tag == "Danger")
        {
            dead = true;
        }
        if(collision.gameObject.tag == "End")
        {
            win = true;
        }
        if(collision.gameObject.tag == "Key")
        {
            exit.GetComponent<KeyCond>().playerKeys += 1;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Pad")
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.3f);
            anim.SetBool("Jumping", true);
            inJump = true;
            jumpCounter = 0;
        }
    }
        
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRad, groundLayer);
    }

    private void Move()
    {
        Vector2 movement = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = movement;
    }
   
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            anim.SetBool("Jumping", true);
            jumpCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            inJump = true;
        }
        else if (rb.velocity.y > 0 && inJump)
        {
            jumpCounter += Time.deltaTime;
            inJump = jumpCounter < jumpTime;
            float temp = jumpCounter / jumpTime;
            float currentJumpMultiplier = jumpMultiplier * (temp - 1);
            rb.velocity += new Vector2(0, -Physics2D.gravity.y) * currentJumpMultiplier * Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            anim.SetBool("Jumping", false);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            inJump = false;
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity -= new Vector2(0, -Physics2D.gravity.y) * fallSpeed * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isOnWall && !IsGrounded())
        {
            anim.SetBool("Jumping", true);
            rb.velocity = new Vector2(jumpForce * direction, jumpForce / 1.5f);
            isOnWall = false;
        }

        /* if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("Jumping", true);
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            anim.SetBool("Jumping", false);//rb.velocity.y / divideJump or 0
        }*/
    }
}
