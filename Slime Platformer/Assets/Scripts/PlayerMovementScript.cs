using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    //movement
    public float speed = 5f;
    public float dir = 0f;
    public float jumpHeight = 15f;
    private bool isFacingRight = true;
    
    //jumping
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //animations
    [SerializeField] private Animator animator;
    private bool isJumping;

    //respawning
    public Vector2 respawnPoint;
    public Text saveText;

    //transformations
    public string currentBody;
    public SpriteRenderer spriteRenderer;  
    public Sprite bunnySprite;
    public Sprite vultureSprite;
    public float bunnyJumpHeight = 25f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        currentBody = "Slime";
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        dir = Input.GetAxis("Horizontal");

        if (dir == 0)
            animator.SetBool("isMoving", false);
        else 
            animator.SetBool("isMoving", true);

        switch(currentBody) {
            case "Slime":
                if (Input.GetButtonDown("Jump") && IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                    animator.SetBool("isGrounded", false);
                    isJumping = true;
                    animator.SetBool("isJumping", true);
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
                break;
            case "Vulture":
                if (Input.GetButtonDown("Jump")) {
                    
                }
                break;

            case "Bunny":
                if (Input.GetButtonDown("Jump") && IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, bunnyJumpHeight);
                    animator.SetBool("isGrounded", false);
                    isJumping = true;
                    animator.SetBool("isJumping", true);
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
                break;
        }

        //test
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentBody = "Slime";
        }

        Flip();

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Death());
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
            StartCoroutine(Death());
        }

        if (col.tag == "Save Spot") {
            Debug.Log("Saved");
            StartCoroutine(Save(col));
        }

        if (col.tag == "Transform Creature") {
            currentBody = col.name;
            if (col.name == "Vulture") {
                Debug.Log("Vulture!");
                //Physics.gravity = new Vector3(0, 0, 0);
                spriteRenderer.sprite = vultureSprite;
            }
            else if (col.name == "Bunny") {
                Debug.Log("Bunny!");
                //Physics.gravity = new Vector3(0, -9.8f, 0);
                spriteRenderer.sprite = bunnySprite;
            }
        }
    }

    IEnumerator Death() {
        animator.SetBool("isDying", true);
        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength * 1.5f);
        animator.SetBool("isDying", false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.position = respawnPoint;
        animator.SetBool("isRespawned", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("isRespawned", false);

    }

    IEnumerator Save(Collider2D save) {
        saveText.enabled = true;
        respawnPoint = save.transform.position;
        yield return new WaitForSeconds(0.5f);
        saveText.enabled = false;
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
