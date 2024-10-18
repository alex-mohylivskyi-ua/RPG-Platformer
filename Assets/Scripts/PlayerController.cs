using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private DefaultPlayerActions defaultPlayerActions;
    [SerializeField] float jumpForce, moveSpeed;
    private Rigidbody2D rb;
    float xInput, yInput;
    private Animator anim;
    private bool isGrounded;
    private float velocity;

    [Header("Attack info")]
    private bool isAttacking;
    private int comboCounter;

    [Header("Combo info")]
    [SerializeField] float comboTime;
    private float comboTimer;

    [Header("Dash info")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldownTime;
    [SerializeField] float dashSpeed;

    [Header("Collision info")]
    private LayerMask groundLayerMask;
    [SerializeField] float groundCheckDistance;

    private float dashTimer;
    private float dashCooldownTimer;

    // private InputAction moveAction;
    // private Vector2 moveDir;
    
    void Awake()
    {
        defaultPlayerActions = new DefaultPlayerActions();
        groundLayerMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        // comboTime = comboTimer;
        // moveAction = defaultPlayerActions.Player.Move;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Flip();
        AnimatorControllers();
        CollisionChecks();
        Timers();
    }

    
    private void CollisionChecks()
    {   
        // isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.5f);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    public void Move()
    {
        if (dashTimer > 0)
        {
            if (xInput == 0)
            {
                rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);
            } else
            {
                rb.velocity = new Vector2(xInput * dashSpeed, 0);
            }
            
        } else 
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    // EVENTS
    public void OnMove(InputAction.CallbackContext context)
    {
        xInput = context.ReadValue<Vector2>().x;
        // moveDir = moveAction.ReadValue<Vector2>();
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if(dashCooldownTimer <= 0 && !isAttacking && context.started)
        {
            Dash();
        }
    }
    public void OnJump(InputAction.CallbackContext context)
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
    public void OnAttack(InputAction.CallbackContext context)
    {
        isAttacking = true;
        comboTimer = comboTime;
    }
    private void OnEnable()
    {
        // Enable new system input events
        defaultPlayerActions.Player.Move.Enable();
        defaultPlayerActions.Player.Jump.Enable();
        defaultPlayerActions.Player.Attack.Enable();
        defaultPlayerActions.Player.Look.Enable();
    }
    private void OnDisable()
    {
        // Disable new system input events
        defaultPlayerActions.Player.Move.Disable();
        defaultPlayerActions.Player.Jump.Disable();
        defaultPlayerActions.Player.Attack.Disable();
        defaultPlayerActions.Player.Look.Disable();
    }
    // EVENTS END
    

    private void Dash()
    {
        dashCooldownTimer = dashCooldownTime;
        dashTimer = dashDuration;
    }

    private void AnimatorControllers()
    {
        bool isMoving = xInput != 0;
        bool isDashing = dashTimer > 0;
        
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", isDashing);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
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

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer < 0)
            {
                comboCounter = 0;
            }
        }
    }

    public void SetAttackOver()
    {
        isAttacking = false;

        comboCounter++;

        if (comboCounter > 2)
        {
            comboCounter = 0;
        }

        // if (comboTimer > 0)
        // {
            
        // } else
        // {
        //     comboCounter = 0;
        // }
    }
}
