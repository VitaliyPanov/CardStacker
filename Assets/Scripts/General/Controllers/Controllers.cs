using System;
using System.Collections.Generic;

namespace CardStacker.General.Controllers
{
    public sealed class Controllers
    {
        private readonly Dictionary<string, IController> _controllersMap;
        private readonly List<IStart> _startControllers;
        private readonly List<IUpdate> _updateControllers;
        private readonly List<IFixedUpdate> _fixedControllers;
        private readonly List<ILateUpdate> _lateControllers;
        private readonly List<IDestroy> _destroyControllers;

        public Controllers()
        {
            _controllersMap = new Dictionary<string, IController>(4);
            _startControllers = new List<IStart>(4);
            _updateControllers = new List<IUpdate>(4);
            _fixedControllers = new List<IFixedUpdate>(4);
            _lateControllers = new List<ILateUpdate>(4);
            _destroyControllers = new List<IDestroy>(4);
        }

        public Controllers Add(IController controller, string name)
        {
            _controllersMap[name] = controller;
            if (controller is IStart initialize)
            {
                _startControllers.Add(initialize);
            }

            if (controller is IUpdate update)
            {
                _updateControllers.Add(update);
            }

            if (controller is IFixedUpdate fixedUpdate)
            {
                _fixedControllers.Add(fixedUpdate);
            }

            if (controller is ILateUpdate lateUpdate)
            {
                _lateControllers.Add(lateUpdate);
            }

            if (controller is IDestroy destroy)
            {
                _destroyControllers.Add(destroy);
            }

            return this;
        }
        public IController Remove(string name)
        {
            if (_controllersMap.TryGetValue(name, out IController controller))
            {
                if (controller is IStart initialize)
                {
                    _startControllers.Remove(initialize);
                }

                if (controller is IUpdate update)
                {
                    _updateControllers.Remove(update);
                }

                if (controller is IFixedUpdate fixedUpdate)
                {
                    _fixedControllers.Remove(fixedUpdate);
                }

                if (controller is ILateUpdate lateUpdate)
                {
                    _lateControllers.Remove(lateUpdate);
                }

                if (controller is IDestroy destroy)
                {
                    _destroyControllers.Remove(destroy);
                }
                return controller;
            }
            return default;
        }

        public void Start()
        {
            for (var index = 0; index < _startControllers.Count; ++index)
            {
                _startControllers[index].Start();
            }
        }

        public void Update()
        {
            for (var index = 0; index < _updateControllers.Count; ++index)
            {
                _updateControllers[index].Update();
            }
        }

        public void FixedUpdate()
        {
            for (var index = 0; index < _fixedControllers.Count; ++index)
            {
                _fixedControllers[index].FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            for (var index = 0; index < _lateControllers.Count; ++index)
            {
                _lateControllers[index].LateUpdate();
            }
        }

        public void Destroy()
        {
            for (var index = 0; index < _destroyControllers.Count; ++index)
            {
                _destroyControllers[index].Destroy();
            }
        }
    }
}