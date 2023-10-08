using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
     private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 75f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool doubleJump;
    private float doubleJumpingPower = 12f;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    public Text winText;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        respawnPoint = transform.position;
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if(isDashing)
        {
            Debug.Log("it got here");
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
            coyoteTimeCounter -= Time.deltaTime;

        if(coyoteTimeCounter > 0f && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (IsGrounded() && !Input.GetButton("Jump")){
            doubleJump = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if(IsGrounded() || doubleJump)
            {
            rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
            coyoteTimeCounter = 0f;

            doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void FixedUpdate()
    {
        if(isDashing){
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        animator.Play("Player_Dash");
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        animator.Play("Player_Idle");
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    private IEnumerator isDead()
    {
        Time.timeScale = 0;;
        animator.Play("Player_Death");
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        animator.Play("Player_Idle");
        Time.timeScale = 1; 
        transform.position = respawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "FallDetector")
        {
            StartCoroutine(isDead());
        }

        if(collision.tag =="Win")
        {
            if(ScoreScript.cheese >=5)
            {
                winText.gameObject.SetActive(true);
            }
        }
    }
}
