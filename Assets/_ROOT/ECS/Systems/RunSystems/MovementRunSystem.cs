using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class MovementRunSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<PositionComponent>().Inc<DirectionComponent>().Inc<SpeedComponent>().End();

            var posPool = world.GetPool<PositionComponent>();
            var dirPool = world.GetPool<DirectionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();

            foreach (var entity in filter)
            {
                ref var position = ref posPool.Get(entity);
                ref var direction = ref dirPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);
                
                var playerSpeed = speed.Speed;

                // Перемещение персонажа
                position.Position += playerSpeed * Time.deltaTime * direction.Direction;

                // Обновление позиции на игровом объекте
                var playerGo = sharedData.SpawnedPlayer;
                playerGo.transform.position = position.Position;

                // Поворот персонажа в сторону движения
                if (direction.Direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.Direction);
                    playerGo.transform.rotation = Quaternion.Lerp(playerGo.transform.rotation, targetRotation, 10 * Time.deltaTime);
                }
            }
        }
    }
}