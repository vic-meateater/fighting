using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// This system is responsible for move player
/// </summary>
namespace Fighting {
    sealed class MovementRunSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<PositionComponent>()
                                      .Inc<DirectionComponent>()
                                      .Inc<SpeedComponent>()
                                      .Inc<PlayerActionComponent>()
                                      .End();

            var posPool = world.GetPool<PositionComponent>();
            var dirPool = world.GetPool<DirectionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            var actionPool = world.GetPool<PlayerActionComponent>();

            foreach (var entity in filter)
            {
                ref var position = ref posPool.Get(entity);
                ref var direction = ref dirPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);
                ref var playerAction = ref actionPool.Get(entity);

                // Проверка на блокировку управления
                if (playerAction.IsControlBlocked)
                    continue;


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