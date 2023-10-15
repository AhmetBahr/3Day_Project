using System;
using System.IO;
using UnityEngine;

namespace Data
{
	public class FileDataHandler
	{
		private string dataDirtPath = "";
		private string dataFileName = "";
		private bool useEncryption = false;
		private readonly string encryptionCodeWord = "word";

		public FileDataHandler(string dataDirtPath, string dataFileName, bool useEncryption)
		{
			this.dataDirtPath = dataDirtPath;
			this.dataFileName = dataFileName;
			this.useEncryption = useEncryption;
		}

		public PlayerData Load()
		{
			string fullPath = Path.Combine(dataDirtPath, dataFileName);
			PlayerData loadedData = null;

			if (File.Exists(fullPath))
			{
				try
				{
					string dataToLoad = "";
					using (FileStream stream = new FileStream(fullPath, FileMode.Open))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							dataToLoad = reader.ReadToEnd();

						}
						
					}

					if (useEncryption)
					{
						dataToLoad = EncryptDecrypt(dataToLoad);
					}

					loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);
				}
				catch (Exception e)
				{
					Debug.LogError("Error occure when trying to load data from file: " + fullPath + "\n" + e);
				}
			}

			return loadedData;
		}

		public void Save(PlayerData data)
		{
			string fullPath = Path.Combine(dataDirtPath, dataFileName);

			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

				string dataToStore = JsonUtility.ToJson(data, true);

				if (useEncryption)
				{
					dataToStore = EncryptDecrypt(dataToStore);
				}

				using (FileStream stream = new FileStream(fullPath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(stream))
					{
						writer.Write(dataToStore);
					}
				}

			}
			catch (Exception e)
			{
				Debug.LogError("Error ocured when trying to save data to file: " + fullPath + "\n" + e);
				
			}
			
		}

		private string EncryptDecrypt(string data)
		{
			string modifiedData = "";

			for (int i = 0; i < data.Length; i++)
			{
				modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
			}

			return modifiedData;
		}
		
	}
}