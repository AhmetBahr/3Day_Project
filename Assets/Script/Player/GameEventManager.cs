using System;
using UnityEngine;

namespace Manager
{
	public class GameEventManager : MonoBehaviour
	{
		public static GameEventManager instance { get; private set; }

		private void Awake()
		{
			if (instance != null)
			{
				Debug.Log("Found more than one Game Events Manager in the scene");
			}
			instance = this;
		}

		public event Action onCoinCollected;
		public void CoinCollected()
		{
			if (onCoinCollected != null)
			{
				onCoinCollected();
			}
		}

	}
}