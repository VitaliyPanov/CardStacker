using System.Linq;
using CardStacker.GameLogic.Components;
using CardStacker.GameLogic.Components.Events;
using CardStacker.GameLogic.Services;
using CardStacker.General.Services.Input;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardStacker.GameLogic
{
    internal sealed class InputSystem : IEcsInitSystem, InputControls.IGeneralActions, IEcsDestroySystem
    {
        private readonly IInputService _inputService;
        private readonly EcsFilterInject<Inc<CardViewComponent, HeroComponent, TransformComponent>> _heroFilter = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        private EcsSystems _systems;
        private Vector2 _pointPosition;
        private Camera _mainCamera;
        public InputSystem(IInputService inputService) => _inputService = inputService;

        public void Init(EcsSystems systems)
        {
            _inputService.AddOrRemoveGeneralListener(this);
            _systems = systems;
            _pointPosition = Vector2.zero;
            _mainCamera = Camera.main;
        }

        public void OnArrows(InputAction.CallbackContext context)
        {
            if (context.started)
                CreateMoveEvent(context.ReadValue<Vector2>());
        }

        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var heroEntity = _heroFilter.Value.GetRawEntities().First();
                var heroPosition = _transformPool.Value.Get(heroEntity).Value.position;
                var direction = new Vector2(_pointPosition.x - heroPosition.x, _pointPosition.y - heroPosition.y).normalized;
                CreateMoveEvent(new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y)));
            }
        }

        public void OnPoint(InputAction.CallbackContext context) => _pointPosition = _mainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

        private void CreateMoveEvent(Vector2 value)
        {
            if (value == Vector2.up) _systems.SetEvent<TryMoveEvent>().Direction = MoveDirection.Up;
            else if (value == Vector2.down)  _systems.SetEvent<TryMoveEvent>().Direction = MoveDirection.Down;
            else if (value == Vector2.right)  _systems.SetEvent<TryMoveEvent>().Direction = MoveDirection.Right;
            else if (value == Vector2.left)  _systems.SetEvent<TryMoveEvent>().Direction = MoveDirection.Left;
        }

        public void Destroy(EcsSystems systems) => _inputService.AddOrRemoveGeneralListener(null);
    }
}