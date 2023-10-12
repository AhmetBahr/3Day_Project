using System;
using Data;
using UnityEngine;

namespace Manager
{
	public class GameManager : MonoBehaviour, IDataPersistence
	{
		public static GameManager instance { get; private set; }

		private void Awake()
		{
			if (instance != null)
			{
				Debug.Log("Found more than one Game manager in this scene");
			}

			instance = this;
		}

		public int playerCoinGM = 0;


		public void LoadData(PlayerData data)
		{
			this.playerCoinGM = data.CoinCount;
		}

		public void SaveData(ref PlayerData data)
		{
			data.CoinCount = this.playerCoinGM;
		}
	}
}