using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
        public static PlayerController instance { get; private set; }
        
        [Header("Core Settings")]
        [SerializeField] private float movementSpeed;
        
        [Header("Check Settings")]
        [SerializeField] private float groundCheckDistance, wallCheckDistance;
        [SerializeField] private Transform wallCheck, groundCheck;
        [SerializeField] private LayerMask whatIsGround, whatIsWall;

        [Header("Jump Settings")]
        [SerializeField] private float jumpPower = 15f;
        [SerializeField] private float coyotoTime = 0.25f;
        private bool doubleJump;
        private float coyoteTimeCounter;
        
        private float jumpCoolDown;

        private int PlayerHeal;
        private int facingDirection, damageDirection;

        private bool isGrounded;
        private bool groundDetected, wallDetected;
        
        private Transform respawnPoint;
        private Vector2 movement;
        
        private GameObject alive;
        private Rigidbody2D aliveRb;
        //private Animator aliveAnim;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Found more than one PlayerController in the scene");

            }

            instance = this;
        }

        private void Start()
        {
            respawnPoint = new GameObject().transform;
            alive = transform.Find("PlayerBoudy").gameObject;
            aliveRb = alive.GetComponent<Rigidbody2D>();
         //   aliveAnim = alive.GetComponent<Animator>();
            PlayerHeal = 1;
            
            facingDirection = 1;
        }

        private void Update()
        {
            UpdateMovingState();
            CheckCoyoteTime();
            UpdateJumoState();


            Buttons();
        }

        private void Buttons()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Damage();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
            
        }
        
        private void UpdateMovingState()
        {
            wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsWall);

            if(wallDetected)
            {
                Flip();
            }
            else
            {
                movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
                aliveRb.velocity = movement;
            }
        }

        private void UpdateJumoState()
        {
            if (CheckGrounded() && !Input.GetKeyDown(KeyCode.Space))
            {
                doubleJump = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (coyoteTimeCounter > 0 || doubleJump)
                {
                    aliveRb.velocity = new Vector2(aliveRb.velocity.x, jumpPower);
                    doubleJump = !doubleJump;
                }

                if (Input.GetKeyDown(KeyCode.Space) && aliveRb.velocity.y > 0f)
                {
                    aliveRb.velocity = new Vector2(aliveRb.velocity.x, aliveRb.velocity.y * 0.5f);
                }
                
            }
            
        }

        private void CheckCoyoteTime()
        {
            if (CheckGrounded())
            {
                coyoteTimeCounter = coyotoTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;

            }
        }
        
        private bool CheckGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, whatIsGround);
        }

        private void EnterDead()
        {
            Destroy(gameObject);
        }
        
        private void Damage()
        {
            PlayerHeal--;

            if(PlayerHeal <= 0)
            {
                EnterDead();
            }
        }
        
        private void Flip()
        {
            facingDirection *= -1;
            alive.transform.Rotate(0.0f, 180.0f, 0.0f);

        }

        public void ChangeRespawnPoint(Transform _transform)
        {
            respawnPoint.transform.position = _transform.position;
            print("Changed reSpawn");
        }
        
        public void Respawn()
        {
            alive.SetActive(true);
            alive.transform.position = respawnPoint.transform.position;
            

        }

        public void PlayerDeath()
        { 
            alive.SetActive(false);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        }
        
	}
}

 
	

