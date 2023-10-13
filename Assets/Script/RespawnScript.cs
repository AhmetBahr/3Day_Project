using System;
using Player;
using UnityEngine;

namespace Manager
{
	public class RespawnScript : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				PlayerController.instance.ChangeRespawnPoint(gameObject.transform);
			}
		}
	}
}