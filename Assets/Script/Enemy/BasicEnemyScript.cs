using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
	public class BasicEnemyScript : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				PlayerController.instance.PlayerDeath();
				
			}
		}
	}
}


