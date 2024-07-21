using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float playerSpeed = 4f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float doubleJumpForce = 6f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minDoubleJumpVelocity = 5;
    [SerializeField] private WallSlideCheck wallSlideCheck;
    [SerializeField] private float wallSlideDrag = 6f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(6f, 7f);
    [SerializeField] private float wallJumpDuration = 0.5f;
    [SerializeField] private float playerHeight;

    public enum PlayerStates { idle, running, jumping, falling, doubleJump, wallSlide }

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private PlayerStates playerState;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool doubleJump = false;
    private float wallJumpCount;

    private void Awake() {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        bool wallSliding = wallSlideCheck.WallSliding() && rigidBody.velocity.y < -0.01f;
        rigidBody.drag = wallSliding ? wallSlideDrag : 0f;
        if (wallJumpCount > 0) {
            rigidBody.velocity -= new Vector2(0, (Time.deltaTime / wallJumpDuration) * wallJumpForce.x);
            wallJumpCount -= Time.deltaTime;
        } else {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            rigidBody.velocity = new Vector2(playerSpeed * horizontalAxis, rigidBody.velocity.y);

            // Make player face the direction we are moving in
            spriteRenderer.flipX = horizontalAxis == 0f ? spriteRenderer.flipX : horizontalAxis < 0f;
        }

        if (Input.GetButtonDown("Jump")) {
            if (IsGrounded()) {
                Jump(jumpForce);
            } else if (rigidBody.velocity.y < minDoubleJumpVelocity && rigidBody.velocity.y > 0f && !doubleJump) {
                Jump(doubleJumpForce);
                doubleJump = true;
            } else if (wallSliding) {
                rigidBody.velocity = spriteRenderer.flipX ?
                    new Vector2(wallJumpForce.x, wallJumpForce.y) :
                    new Vector2(-wallJumpForce.x, wallJumpForce.y);
                spriteRenderer.flipX = !spriteRenderer.flipX;
                wallJumpCount = wallJumpDuration;
            }
        }

        doubleJump = doubleJump && rigidBody.velocity.y >= 0f;

        // Set player animation state
        playerState = wallSliding ? PlayerStates.wallSlide :
            doubleJump ? PlayerStates.doubleJump :
            rigidBody.velocity.y > 0.01f ? PlayerStates.jumping :
            rigidBody.velocity.y < -0.01f ? PlayerStates.falling :
            rigidBody.velocity.x == 0f ? PlayerStates.idle : PlayerStates.running;

        animator.SetInteger("playerState", (int)playerState);

    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.01f, layerMask);
    }

    public PlayerStates GetPlayerState() {
        return playerState;
    }

    public void Jump(float force) {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, force);
    }

}