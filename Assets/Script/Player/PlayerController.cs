using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
        public int PlayerHeal;
        [SerializeField] private float movementSpeed;
        
        [SerializeField] private float groundCheckDistance, wallCheckDistance;
        [SerializeField] private Transform wallCheck, groundCheck;
        [SerializeField] private LayerMask whatIsGround, whatIsCheck;
        
        private int facingDirection, damageDirection;
        private Vector2 movement;
        private bool groundDetected, wallDetected;
        
        private GameObject alive;
        private Rigidbody2D aliveRb;
        //private Animator aliveAnim;
        
        private void Start()
        {
            alive = transform.Find("PlayerBoudy").gameObject;
            aliveRb = alive.GetComponent<Rigidbody2D>();
         //   aliveAnim = alive.GetComponent<Animator>();
            PlayerHeal = 1;
            
            facingDirection = 1;
        }

        private void Update()
        {
            UpdateMovingState();
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Damage();
            }

        }
        
        private void UpdateMovingState()
        {
            wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

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

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        }
	}
}

 
	

