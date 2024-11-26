using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Animator anim;
    private bool isFacingRight = true;
    private float dirX = 0f;

    private bool isWallSlide;
    private float wallSlideSpeed = 2f;
    private float wallSlideTimer = 0f; // Timer for wall slide
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private float jumpCooldown = 0.25f; // Cooldown time in seconds
    private float jumpCooldownTimer = 0f; // Timer to track the cooldown
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    public iInteractable Interactable { get; set; }

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;

    private bool isGrounded;
    private int remainingJumps;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.freezeRotation = true;

        remainingJumps = 1;
    }

private void Update()
{
    if (!GameManager.instance.GameOver)
    {
        if (dialogueUI.isOpen) return;

        dirX = Input.GetAxisRaw("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(dirX));
        
        isGrounded = IsGrounded();
        isWallSlide = isWalled();
        //print(isWallSlide);
        //print(remainingJumps);

        if (isGrounded)
        {
            remainingJumps = 1;
        }

        // Only allow jumping if the cooldown timer has elapsed
        if (jumpCooldownTimer <= 0f && Input.GetButtonDown("Jump"))
        {
            if (isGrounded || remainingJumps > 0)
            {
                if (!isGrounded)
                {
                    remainingJumps--;
                }
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpSoundEffect.Play();
                jumpCooldownTimer = jumpCooldown; // Reset the cooldown timer after the jump
                //anim.SetBool("IsJumping", true);
            }
        }

        if (isWallSlide)
        {
            remainingJumps = 1;
        }

        // Only allow wall jump if the cooldown timer has elapsed
        if (jumpCooldownTimer <= 0f && Input.GetButtonDown("Jump"))
        {
            if (isWallSlide || remainingJumps > 0)
            {
                if (!isWallSlide)
                {
                    remainingJumps--;
                }
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpSoundEffect.Play();
                jumpCooldownTimer = jumpCooldown; // Reset the cooldown timer after the jump
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }

        wallSlide();
        wallJump();

        if (isWallJumping)
        {
            Jump();
        }

        UpdateAnimationState();
    }

    // Update the cooldown timer
    if (jumpCooldownTimer > 0f)
    {
        jumpCooldownTimer -= Time.deltaTime;
    }
}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

    private void wallSlide()
    {
        if (isWalled() && !IsGrounded() && dirX != 0)  // Ensure we're moving along the wall
        {
            isWallSlide = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));

            // Make sure the character is facing the wall
            if (dirX > 0f && !isFacingRight) Flip(); // Wall is to the right, but character faces left
            else if (dirX < 0f && isFacingRight) Flip(); // Wall is to the left, but character faces right

            // Start or reset the wall slide timer
            wallSlideTimer += Time.deltaTime;
        
            // If the wall slide timer exceeds 0.25 seconds, reset remaining jumps
            if (wallSlideTimer >= 0.25f)
            {
                remainingJumps = 1;
            }
        }
        else
        {
            isWallSlide = false;
            wallSlideTimer = 0f; // Reset the timer when not wall sliding
        }
    }

    private void wallJump()
    {
        if (isWallSlide)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;  // Set direction opposite to facing
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            // Apply the wall jump force
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            // Flip the character if necessary
            if (transform.localScale.x != wallJumpingDirection)
            {
                Flip();
            }

            // Update the character's velocity to move in the correct direction after flipping
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, rb.velocity.y);

            // Optionally: Apply a small push away from the wall after jumping (if necessary)
            transform.position += new Vector3(wallJumpingDirection * 0.1f, 0f, 0f); // Small horizontal push

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;  // Flip the character horizontally
        transform.localScale = localScale;
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void Jump()
    {
        if (isFacingRight && dirX < 0f || isFacingRight && dirX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
}