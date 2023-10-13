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
				Debug.Log("Found more than one Data Persistence Manager in the scene");
			}

			instance = this;
		}

		private void Start()
		{
			this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
			this._dataPersistences = FindAllDataPersistenceObjects();
			LoadGame();
		}

		public void NewGame()
		{
			this._playerData = new PlayerData();

		}

		public void LoadGame()
		{
			// Load any saved data from a file using
			this._playerData = _fileDataHandler.Load();

			if (this._playerData == null)
			{
				Debug.Log("No data was found.");
				NewGame();
			}
			//ToDo push the loaded data all other scriots that need it
			foreach (IDataPersistence dataPersistenceObj in _dataPersistences)
			{
				dataPersistenceObj.LoadData(_playerData);
			}
		}

		public void SaveGame()
		{
			//ToDo pass the data to other scripts so they can uptade it 
			foreach (IDataPersistence dataPersistenceObj in _dataPersistences)
			{
				dataPersistenceObj.SaveData(ref _playerData);
			}
			
			// save that data to a file using the data handler 
			_fileDataHandler.Save(_playerData);
		}

		private void OnApplicationQuit()
		{
			SaveGame();
		}

		private List<IDataPersistence> FindAllDataPersistenceObjects()
		{
			IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
				.OfType<IDataPersistence>();
			
			return new List<IDataPersistence>(dataPersistenceObjects);
		}
	}
}