using UnityEngine;

public class Player : MonoBehaviour
{
    public bool grounded = true;
    private Rigidbody2D rb2d;
    public float jumpPower;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask platformsLayerMask;

    private int jumpCount;
    public int jumpCountMax;
    public float speed = 5f;

    void Start()
    {
        rb2d = rb2d = GetComponent<Rigidbody2D> ();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        jumpCountMax = 2;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;

        if (IsGrounded()) {
            jumpCount = 1;
        }

        if(Input.GetKey(KeyCode.Space)) {
            if (IsGrounded()) {
                rb2d.velocity = Vector2.up * jumpPower;

            } else {
                if(Input.GetKeyDown(KeyCode.Space)) {
                    if (jumpCount < jumpCountMax) {
                        rb2d.velocity = Vector2.up * jumpPower;
                        jumpCount++;
                    }
                }
            }
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycastHit2D.collider != null;
    }
}
