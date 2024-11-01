using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class EnemyFollowRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();

            // Фильтр для нахождения игрока и получения его позиции
            var playerFilter = world.Filter<PlayerTagComponent>().End();
            var playerPosition = Vector3.zero;
            foreach (var playerEntity in playerFilter) {
                ref var playerTransform = ref world.GetPool<PositionComponent>().Get(playerEntity);
                playerPosition = playerTransform.Position;
                break;
            }

            // Фильтр для врагов
            var enemyFilter = world.Filter<EnemyTagComponent>()
                                      .Inc<PositionComponent>()
                                      .Inc<SpeedComponent>()
                                      .Inc<EnemyStateComponent>()
                                      .Inc<GameObjectComponent>()
                                      .End();
            var positionPool = world.GetPool<PositionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            var statePool = world.GetPool<EnemyStateComponent>();
            var gameObjectPool = world.GetPool<GameObjectComponent>();
            
            float stopDistance = 1.5f;

            foreach (var enemyEntity in enemyFilter) {
                ref var enemyTransform = ref positionPool.Get(enemyEntity);
                ref var enemySpeed = ref speedPool.Get(enemyEntity);
                ref var enemyState = ref statePool.Get(enemyEntity);
                ref var enemyGameObject = ref gameObjectPool.Get(enemyEntity);
                
                float distanceToPlayer = Vector3.Distance(playerPosition, enemyTransform.Position);
                var saveEnemySpeed = enemySpeed.Speed;
                if (distanceToPlayer <= stopDistance)
                {
                    enemySpeed.Speed = 0;
                    //enemyState.State = EnemyState.Idle;
                }
                else
                {
                    enemySpeed.Speed = 1;
                    enemyState.State = EnemyState.Following;

                    // Направление на игрока и движение к нему
                    var directionToPlayer = (playerPosition - enemyTransform.Position).normalized;
                    enemyTransform.Position += directionToPlayer * enemySpeed.Speed * Time.deltaTime;

                    // Поворот врага к игроку
                    if (directionToPlayer != Vector3.zero)
                    {
                        enemyTransform.Rotation = Quaternion.LookRotation(directionToPlayer);
                    }
                    
                    var go = enemyGameObject.gameObject;
                    if (go)
                    {
                        go.transform.position = enemyTransform.Position;
                        go.transform.rotation = enemyTransform.Rotation;
                    }
                }
            }
        }
    }
}