using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviorRanged : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    public float jumpPower;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask platformsLayerMask;
    public float time = 0;
    private float timeStore;

    public Transform target;
    private int moveSpeed = 2;

    private float moveDirection = -1;
    private bool facingRight = false;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private float circleRadius = 0.2f;
    [SerializeField] private bool checkingGround;
    [SerializeField] private bool checkingWall;
    public Transform myTransform;

    [SerializeField] float jumpHeight;
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;

    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] bool canSeePlayer;

    [SerializeField] GameObject bullet;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        myTransform = transform;
        target = GameObject.FindWithTag("Player").transform;
        timeStore = time;
    }

    void FixedUpdate()
    {
        canSeePlayer = Physics2D.OverlapBox(myTransform.position, lineOfSight, 0, playerLayer);
        checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, platformsLayerMask);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, platformsLayerMask);

        if (!canSeePlayer && IsGrounded())
        {
            patrolling();
            time = 1.5f;
        }
        else
        {
            if (time < 2)
            {
                time += Time.deltaTime;
            }
            else
            {
                rangedAttack();
                time = timeStore;
            }
        }
    }

    void rangedAttack()
    {
        Instantiate(bullet, myTransform.position, Quaternion.identity);
    }

    void patrolling()
    {
        if (!checkingGround || checkingWall)
        {
            if (facingRight)
            {
                Flip();
            }
            else if (!facingRight)
            {
                Flip();
            }
        }
        rb2d.velocity = new Vector2(moveSpeed * moveDirection, rb2d.velocity.y);
    }

    void flipTowardsPlayer()
    {
        float playerPostition = target.position.x - myTransform.position.x;
        if (playerPostition < 0 && facingRight == true)
        {
            Flip();
        }
        else if (playerPostition > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
}
