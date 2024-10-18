using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    protected Rigidbody2D rb;
    protected Animator anim;
    protected LayerMask groundLayerMask;
    protected bool isGrounded;

    [Header("Move info")]
    [SerializeField] protected float jumpForce, moveSpeed;
    
    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        groundLayerMask = groundLayerMask = LayerMask.GetMask("Ground");
    }

    protected virtual void CollisionChecks()
    {   
        // isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.5f);
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);
    }

    protected virtual void Flip()
    {
        // Update player animation
        if (rb.velocity.x < 0) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (rb.velocity.x > 0) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // It's unity function
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}
