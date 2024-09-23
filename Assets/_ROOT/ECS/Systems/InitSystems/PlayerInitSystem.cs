using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// This system is used to initialize and spwan the player
/// </summary>
namespace Fighting {
    sealed class PlayerInitSystem : IEcsInitSystem {
        public void Init (IEcsSystems systems) {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            
            var playerEntity = world.NewEntity();

            var playerPool = world.GetPool<PlayerTagComponent>();
            playerPool.Add(playerEntity);

            var directionPool = world.GetPool<DirectionComponent>();
            directionPool.Add(playerEntity);

            var playerActionPool = world.GetPool<PlayerActionComponent>();
            playerActionPool.Add(playerEntity);

            var animatorPool = world.GetPool<AnimatorComponent>();
            animatorPool.Add(playerEntity);
            ref var animatorComponent = ref animatorPool.Get(playerEntity);

            var speedPool = world.GetPool<SpeedComponent>();
            speedPool.Add(playerEntity);
            ref var speedComponent = ref speedPool.Get(playerEntity);

            var positionPool = world.GetPool<PositionComponent>();
            positionPool.Add(playerEntity);
            ref var positionComponent = ref positionPool.Get(playerEntity);

            //Спавним игрока
            var playerGo = GameObject.Instantiate(sharedData.PlayerConfig.Prefab, 
                                                    sharedData.PlayerConfig.SpawnPoint.transform.position, 
                                                    Quaternion.identity);

            sharedData.SpawnedPlayer = playerGo;
            speedComponent.Speed = sharedData.PlayerConfig.Speed;
            positionComponent.Position = playerGo.transform.position;
            animatorComponent.Animator = playerGo.GetComponent<Animator>();
        }
    }
}