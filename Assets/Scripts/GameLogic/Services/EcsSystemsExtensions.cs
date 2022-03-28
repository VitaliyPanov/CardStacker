using Leopotam.EcsLite;
using UnityEngine;

namespace CardStacker.GameLogic.Services
{
    public static class EcsSystemsExtensions
    {
        public static ref T AddOrRefreshEntity<T>(this EcsSystems systems, int entity) where T : struct
        {
            var world = systems.GetWorld();
            var pool = world.GetPool<T>();
            if (pool.Has(entity))
                pool.Del(entity);
            ref var eventComponent = ref pool.Add(entity);
            return ref eventComponent;
        }

        public static ref T SetEvent<T>(this EcsSystems systems, bool isNewEntity = true, int entity = default)
            where T : struct
        {
            var eventsWorld = systems.GetWorld(WorldNames.EVENTS);
            var eventEntity = isNewEntity ? eventsWorld.NewEntity() : entity;
            return ref eventsWorld.GetPool<T>().Add(eventEntity);
        }

        public static void SetTimer(this EcsSystems systems, int entity, float lifeTime)
        {
            var world = systems.GetWorld();
            ref var timerComponent = ref world.GetPool<TimerComponent>().Add(entity);
            timerComponent = new TimerComponent(typeof(TimerComponent), entity, lifeTime + Time.time);
        }

        public static void SetTimerAtPool<T>(this EcsSystems systems, float lifeTime, int entity,
            bool isNewEntity = false) where T : struct
        {
            var world = systems.GetWorld();
            var eventEntity = isNewEntity ? world.NewEntity() : entity;
            ref TimerComponent timerComponent = ref world.GetPool<TimerComponent>().Add(eventEntity);
            timerComponent = new TimerComponent(typeof(T), entity, lifeTime + Time.time);
        }

        public static EcsSystems AddOneFrameSystem<T>(this EcsSystems systems, string worldName = null) where T : struct
        {
            return systems.Add(new OneFrameSystem<T>(systems.GetWorld(worldName)));
        }

        public static EcsSystems AddTimeTrippingSystem(this EcsSystems systems, string worldName = null)
        {
            return systems.Add(new TimeTrippingSystem(systems.GetWorld(worldName)));
        }
    }
}