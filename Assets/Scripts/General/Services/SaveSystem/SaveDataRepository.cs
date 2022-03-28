using System;
using System.IO;
using UnityEngine;

namespace CardStacker.General.Services.SaveSystem
{
    public sealed class SaveDataRepository<T> : ISaveDataService<T>
    {
        private static readonly string s_savingRepository = Application.dataPath + "/Saves/";
        private readonly IData<T> _savingSystem;
        private readonly string _key;
        private readonly string _savingFolder;

        public SaveDataRepository(SavingType type, string folderToSave = null, string key = default)
        {
            _key = key;
            _savingSystem = GetSavingType(type);
            _savingFolder = s_savingRepository + folderToSave + "/";
            if (!Directory.Exists(_savingFolder))
            {
                Directory.CreateDirectory(_savingFolder);
            }
        }

        public void Save(T saveData, bool overWrite = false)
        {
            int saveNumber = 1;
            if (overWrite) File.Delete(_savingFolder + "save_" + saveNumber + ".txt");
            while (File.Exists(_savingFolder + "save_" + saveNumber + ".txt"))
            {
                saveNumber++;
            }

            _savingSystem.Save(saveData, _savingFolder + "save_" + saveNumber + ".txt", _key);
        }

        public T Load()
        {
            var mostRecentFile = GetMostRecentFile();
            if (mostRecentFile == null) return default;
            var loadFile = _savingSystem.Load(mostRecentFile.FullName, _key);
            return loadFile;
        }

        private FileInfo GetMostRecentFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_savingFolder);
            FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
            FileInfo mostRecentFile = null;
            foreach (var fileInfo in saveFiles)
            {
                if (mostRecentFile == null)
                    mostRecentFile = fileInfo;
                else
                {
                    if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                        mostRecentFile = fileInfo;
                }
            }

            return mostRecentFile;
        }

        private static IData<T> GetSavingType(SavingType type)
        {
            return (type) switch
            {
                SavingType.Json => new JsonData<T>(),
                SavingType.JsonCrypto => new JsonCryptoData<T>(),
                SavingType.XML => new SerializableXMLData<T>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void Overwrite(T data)
        {
            var mostRecentFile = GetMostRecentFile();
            if (mostRecentFile == null) return;
            _savingSystem.LoadOverwrite(mostRecentFile.FullName, data, _key);
        }
    }
}