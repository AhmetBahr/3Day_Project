using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
	public class DataPersentationManager : MonoBehaviour
	{
		[Header("File Storage Config")] 
		[SerializeField] private string fileName;
		[SerializeField] private bool useEncryption;

		private PlayerData _playerData;
		private List<IDataPersistence> _dataPersistences;
		private FileDataHandler _fileDataHandler;

		public static DataPersentationManager instance { get; private set; }

		private void Awake()
		{
			if (instance != null)
			{
				Debug.Log("Found more than  one Data persistence manager in the scene");
			}

			instance = this;
		}

		private void Start()
		{
			this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
		//	this._dataPersistences = 
			LoadGame();	

		}

		public void NewGame()
		{
			this._playerData = new PlayerData();
		}

		public void LoadGame()
		{
			this._playerData = _fileDataHandler.Load();

			if (this._playerData == null)
			{
				Debug.Log("No data was found");
				NewGame();
			}

			foreach (IDataPersistence dataPersistence in _dataPersistences)
			{
				dataPersistence.LoadData(_playerData);
			}
			
		}

		public void SaveGame()
		{
			foreach (IDataPersistence dataPersistence in _dataPersistences)
			{
				dataPersistence.SaveData(ref _playerData);
				
			}
			_fileDataHandler.Save(_playerData);
			
		}

		private void onApplicattionQuit()
		{
			SaveGame();
		}

		private List<IDataPersistence> FindAllDataPersistenceObjects()
		{
			IEnumerable<IDataPersistence> dataPersistences =
				FindObjectsOfType<MonoBehaviour>()
					.OfType<IDataPersistence>();


			return new List<IDataPersistence>(_dataPersistences);
		}
	}
}