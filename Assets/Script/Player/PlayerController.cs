using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
        public static PlayerController instance { get; private set; }
        
        public int PlayerHeal;
        [SerializeField] private float movementSpeed;
        
        [SerializeField] private float groundCheckDistance, wallCheckDistance;
        [SerializeField] private Transform wallCheck, groundCheck;
        [SerializeField] private LayerMask whatIsGround, whatIsWall;

        [SerializeField] private float jumpPower = 15f;
        [SerializeField] private int extraJump = 1;

        private Transform respawnPoint;

        private int jumpCount = 0;
        private bool isGrounded;
        private float jumpCoolDown;

        private int facingDirection, damageDirection;
        private Vector2 movement;
        private bool groundDetected, wallDetected;
        
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
            UpdateJumoState();
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Damage();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }

            CheckGrounded();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded || jumpCount < extraJump)
                {
                    aliveRb.velocity = new Vector2(aliveRb.velocity.x, jumpPower);
                    jumpCount++;
                }
            }
        }

        private void CheckGrounded()
        {
            if (Physics2D.OverlapCircle(groundCheck.position, 0.5f, whatIsGround))
            {
                isGrounded = true;
                jumpCount = 0;
                jumpCoolDown = Time.time + 0.2f;
            }
            else if (Time.time < jumpCoolDown)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
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

 
	

