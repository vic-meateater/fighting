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
            ref var playerAction = ref playerActionPool.Get(playerEntity);

            var animatorPool = world.GetPool<AnimatorComponent>();
            animatorPool.Add(playerEntity);
            ref var animatorComponent = ref animatorPool.Get(playerEntity);

            var speedPool = world.GetPool<SpeedComponent>();
            speedPool.Add(playerEntity);
            ref var speedComponent = ref speedPool.Get(playerEntity);

            var positionPool = world.GetPool<PositionComponent>();
            positionPool.Add(playerEntity);
            ref var positionComponent = ref positionPool.Get(playerEntity);
            
            var playerStatePool = world.GetPool<PlayerStateComponent>();
            playerStatePool.Add(playerEntity);
            ref var playerState = ref playerStatePool.Get(playerEntity);

            //Spawn player
            var playerGO = GameObject.Instantiate(sharedData.PlayerConfig.Prefab, 
                                                    sharedData.PlayerConfig.SpawnPoint.transform.position, 
                                                    Quaternion.identity);

            sharedData.SpawnedPlayer = playerGO;
            speedComponent.Speed = sharedData.PlayerConfig.Speed;
            positionComponent.Position = playerGO.transform.position;
            animatorComponent.Animator = playerGO.GetComponentInChildren<Animator>();
            playerState.State = PlayerState.Idle;
            
            //var entityLink = playerGO.AddComponent<EntityLink>();
            sharedData.PlayerConfig.EntityID = playerEntity;
            
            var playerColliders = playerGO.GetComponent<PlayerDamageableColliders>();
            playerAction.LeftArm = playerColliders.LeftArm;
            playerAction.LeftArm.SetActive(false);
            playerAction.RightArm = playerColliders.RightArm;
            playerAction.RightArm.SetActive(false);
            playerAction.LeftLeg = playerColliders.LeftLeg;
            playerAction.LeftLeg.SetActive(false);
            playerAction.RightLeg = playerColliders.RightLeg;
            playerAction.RightLeg.SetActive(false);
        }
    }
}