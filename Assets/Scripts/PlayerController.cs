using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce, moveSpeed;
    private Rigidbody2D rb;
    float xInput, yInput;
    private Animator anim;
    private bool isGrounded;

    [Header("Attack info")]
    private bool isAttacking;
    private int comboCounter;

    [Header("Dash info")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldownTime;
    [SerializeField] float dashSpeed;

    [Header("Collision info")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float groundCheckDistance;

    private float dashTimer;
    private float dashCooldownTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Movement();
        Flip();
        AnimatorControllers();
        CollisionChecks();
        Timers();
        
        // transform.Translate(new Vector3(xInput, yInput, 0) * moveSpeed * Time.deltaTime);
        //Get the value of the Horizontal input axis.
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Jump();
        // }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            Dash();
        }
    }

    private void Movement()
    {
        if (dashTimer > 0)
        {
            if (xInput == 0)
            {
                rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
            } else
            {
                rb.velocity = new Vector2(xInput * dashSpeed, rb.velocity.y);
            }
            
        } else 
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Dash()
    {
        dashCooldownTimer = dashCooldownTime;
        dashTimer = dashDuration;
    }

    public void Jump(InputAction.CallbackContext context)
    {   
        if (context.started && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (context.canceled && !isGrounded && rb.velocity.y > 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            Debug.Log("ALOHA");
        }
    }

    public void Jump2(InputAction.CallbackContext context) {
        if (Time.timeScale != 0)
        {
            if (context.started && isGrounded) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                // AudioManager.instance.PlaySFX(1);
            }

            // Make small jump
            if (context.canceled && !isGrounded && rb.velocity.y > 0) 
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            }
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = xInput != 0;
        bool isDashing = dashTimer > 0;
        
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", isDashing);
    }

    private void Flip()
    {
        // Update player animation
        if (rb.velocity.x < 0) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (rb.velocity.x > 0) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // It's unity function
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    private void Timers()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
}
