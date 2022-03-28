using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace CardStacker.GameLogic.Services
{
    internal static class WorldNames
    {
        public const string GAME = "game";
        public const string EVENTS = "events";
    }

    public sealed class OneFrameSystem<T> : IEcsInitSystem, IEcsRunSystem where T : struct
    {
        private readonly EcsFilter _filter;
        private readonly EcsPool<T> _pool;

        public OneFrameSystem(EcsWorld world)
        {
            _filter = world.Filter<T>().End();
            _pool = world.GetPool<T>();
        }

        public void Init(EcsSystems systems) => RemoveEvent();

        public void Run(EcsSystems systems) => RemoveEvent();

        private void RemoveEvent()
        {
            foreach (var i in _filter)
            {
                _pool.Del(i);
            }
        }
    }

    public sealed class TimeTrippingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter _filter;
        private readonly EcsPool<TimerComponent> _pool;

        public TimeTrippingSystem(EcsWorld world)
        {
            _world = world;
            _filter = world.Filter<TimerComponent>().End();
            _pool = world.GetPool<TimerComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var timerComponent = ref _pool.Get(i);
                if (timerComponent.TrippingTime > Time.time)
                    continue;

                var componentPool = _world.GetPoolByType(timerComponent.Component);
                if (timerComponent.Entity != i)
                {
                    componentPool.Del(timerComponent.Entity);
                }

                _pool.Del(i);
            }
        }
    }

    public readonly struct TimerComponent
    {
        public Type Component { get; }
        public int Entity { get; }
        public float TrippingTime { get; }

        public TimerComponent(Type component, int entity, float trippingTime)
        {
            Component = component;
            Entity = entity;
            TrippingTime = trippingTime;
        }
    }
}