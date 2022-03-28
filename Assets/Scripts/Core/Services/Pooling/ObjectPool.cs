using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardStacker.Core.Services.Pooling
{
    internal sealed class ObjectPool : IDisposable
    {
        private readonly Stack<GameObject> _stack = new Stack<GameObject>(64);
        private readonly GameObject _prefab;
        private readonly Transform _root;

        public ObjectPool(GameObject prefab)
        {
            _prefab = prefab;
            _root = new GameObject($"[{_prefab.name}]").transform;
        }
        public GameObject Pop(Transform parent = null)
        {
            GameObject gameObject;
            if (_stack.Count == 0)
            {
                gameObject = parent != null ? Object.Instantiate(_prefab, parent) : Object.Instantiate(_prefab);
                gameObject.name = _prefab.name;
            }
            else
            {
                gameObject = _stack.Pop();
                if (parent != null)
                {
                    gameObject.transform.position = parent.position;
                    gameObject.transform.rotation = parent.rotation;
                }
            }
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(true);
            return gameObject;
        }

        public void Push(GameObject gameObject)
        {
            _stack.Push(gameObject);
            gameObject.transform.SetParent(_root);
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            for (int i = 0; i < _stack.Count; i++)
            {
                GameObject gameObject = _stack.Pop();
                Object.Destroy(gameObject);
            }
            Object.Destroy(_root.gameObject);
        }
    }
}