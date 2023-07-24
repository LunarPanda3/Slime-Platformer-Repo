using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementScript : MonoBehaviour
{
    //movement
    public float speed = 5f;
    public float dir = 0f;
    public float jumpHeight = 30f;
    private bool isFacingRight = true;
    
    //jumping
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //animations
    [SerializeField] private Animator animator;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dir = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            animator.SetBool("isGrounded", false);
            isJumping = true;
            animator.SetBool("isJumping", true);
            //jumpAnim.Play("Blue Jump");
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }

        if (isJumping && rb.velocity.y < 0f) {
            animator.SetBool("isFalling", true);
        }

        if (IsGrounded() && rb.velocity.y == 0f){
            animator.SetBool("isGrounded", true);
            isJumping = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        Flip();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Kill Axis") {
            Reload();
        }
    }

    void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Flip() {
        if (isFacingRight && dir < 0f || !isFacingRight && dir > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
