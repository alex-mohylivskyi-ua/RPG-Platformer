using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, 0);
        if(!isGrounded || isWallDetected)
        {
            Debug.Log("Flip");
            // Flip();
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
