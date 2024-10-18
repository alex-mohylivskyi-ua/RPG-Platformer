using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : Entity
{
    private DefaultPlayerActions defaultPlayerActions;
    
    float xInput, yInput;
    

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

    
    

    private float dashTimer;
    private float dashCooldownTimer;

    // private InputAction moveAction;
    // private Vector2 moveDir;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
        defaultPlayerActions = new DefaultPlayerActions();
    }


    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
        Flip();
        AnimatorControllers();
        
        Timers();
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
