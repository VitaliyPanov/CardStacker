using System.IO;
using UnityEngine;

namespace CardStacker.General.Services.SaveSystem
{
    public class JsonData<T> : IData<T>
    {
        public void Save(T data, string path, string key = default)
        {
            string saveStr = JsonUtility.ToJson(data);
            File.WriteAllText(path, saveStr);
        }

        public T Load(string path, string key = default)
        {
            string saveStr = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(saveStr);
        }

        public void LoadOverwrite(string path, T target, string key)
        {
            string saveStr = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(saveStr, target);
        }
    }
}