using Manager;
using UnityEngine;

namespace Collectable
{
	public class Coin_Script : MonoBehaviour
	{
		[SerializeField] private int pointCount;
		
		//ToDo anim
		//ToDo particul 


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				gameObject.SetActive(false);
				CoinCollected();
			}
		}

		private void CoinCollected()
		{
			//ToDo Data Manager 
			GameManager.instance.playerCoinGM++;

		}
		
	}

}
