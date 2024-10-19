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
    protected LayerMask wallLayerMask;
    protected bool isGrounded;
    protected bool isWallDetected;

    [Header("Move info")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpForce;
    
    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
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
        // groundLayerMask = groundLayerMask = LayerMask.GetMask("Ground");
        // wallLayerMask = wallLayerMask = LayerMask.GetMask("Wall");
        groundLayerMask = LayerMask.GetMask("Ground");
        // TODO get correct layer
        wallLayerMask = LayerMask.GetMask("Ground");
    }

    protected virtual void CollisionChecks()
    {   
        // isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.5f);
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * transform.localScale.x, wallLayerMask);
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
        if (groundCheck != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }

        if (wallCheck != null)
        {   
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (wallCheckDistance * transform.localScale.x), wallCheck.position.y));
        }
    }
}
