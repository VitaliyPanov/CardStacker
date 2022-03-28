using System.IO;
using UnityEngine;

namespace CardStacker.General.Services.SaveSystem
{
    public class JsonCryptoData<T> : IData<T>
    {

        public void Save(T data, string path, string key)
        {
            string saveStr = JsonUtility.ToJson(data);
            var encryptedData = AesCryptoService.AesEncrypt(saveStr, key);
            File.WriteAllText(path, encryptedData);
        }

        public T Load(string path, string key)
        {
            string saveStr = File.ReadAllText(path);
            var decryptedData = AesCryptoService.AesDecrypt(saveStr, key);
            return JsonUtility.FromJson<T>(decryptedData);
        }

        public void LoadOverwrite(string path, T target, string key)
        {
            string saveStr = File.ReadAllText(path);
            var decryptedData = AesCryptoService.AesDecrypt(saveStr, key);
            JsonUtility.FromJsonOverwrite(decryptedData, target);
        }
    }
}