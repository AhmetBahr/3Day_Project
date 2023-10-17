using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance { get; private set; }

        private float movementInputDirection = 1;

        private int amountOfJumpsLeft;
        private int facingDirection = 1;

        private bool isFacingRight = true;
        private bool isWalking;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isWallSliding;
        private bool canJump;

        private Rigidbody2D rb;
        private Animator anim;

        public int amountOfJumps = 1;

        public float movementSpeed = 10.0f;
        public float jumpForce = 16.0f;
        public float groundCheckRadius;
        public float wallCheckDistance;
        public float wallSlideSpeed;
        public float movementForceInAir;
        public float variableJumpHeightMultiplier = 0.5f;
        public float wallHopForce;
        public float wallJumpForce;

        public Vector2 wallHopDirection;
        public Vector2 wallJumpDirection;

        public Transform groundCheck;
        public Transform wallCheck;

        public LayerMask whatIsGround;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
         //   anim = GetComponent<Animator>();
            amountOfJumpsLeft = amountOfJumps;
            wallHopDirection.Normalize();
            wallJumpDirection.Normalize();
        }

        // Update is called once per frame
        void Update()
        {
            CheckInput();
            CheckMovementDirection();
          //  UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            CheckSurroundings();
        }

        private void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
            {
                isWallSliding = true;
                movementSpeed = 0;

            }
            else
            {
                isWallSliding = false;
                movementSpeed = 10;

            }
        }

        private void CheckSurroundings()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        }

        private void CheckIfCanJump()
        {
            if ((isGrounded && rb.velocity.y <= 0) || isWallSliding)
            {
                amountOfJumpsLeft = amountOfJumps;
            }

            if (amountOfJumpsLeft <= -1)
            {
                canJump = false;
            }
            else
            {
                canJump = true;
            }

        }

        private void CheckMovementDirection()
        {
            if (isFacingRight && movementInputDirection < 0)
            {
                Flip();
            }
            else if (!isFacingRight && movementInputDirection > 0)
            {
                Flip();
            }

            if (rb.velocity.x != 0)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }

        private void UpdateAnimations()
        {
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.velocity.y);
            anim.SetBool("isWallSliding", isWallSliding);
        }

        private void CheckInput()
        {

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonUp("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }

        }

        private void Jump()
        {
            if (canJump && !isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                amountOfJumpsLeft--;
            }
            else if (isWallSliding && movementSpeed == 0 && canJump) //Wall hop
            {
                isWallSliding = false;
                amountOfJumpsLeft--;
                Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
                rb.AddForce(forceToAdd, ForceMode2D.Impulse);
                Flip();
            }
            else if ((isWallSliding || isTouchingWall) && movementSpeed != 0 && canJump)
            {
                isWallSliding = false;
                amountOfJumpsLeft--;
                Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
                rb.AddForce(forceToAdd, ForceMode2D.Impulse);
                Flip();
            }
        }

        private void ApplyMovement()
        {

            if (isGrounded)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
            else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
            {
                XJump();
            }

            if (isWallSliding)
            {
                if (rb.velocity.y < -wallSlideSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                }
            }
        }

        private void XJump()
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }

        private void Flip()
        {
            if (!isWallSliding)
            {
                movementInputDirection *= -1; 
                facingDirection *= -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

            Gizmos.DrawLine(wallCheck.position,
                new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        }
    }
}

 
	

