using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class EnemySpawnInitRunSystem : IEcsInitSystem, IEcsRunSystem 
    {
        public void Init (IEcsSystems systems) 
        {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            //Spawn enemy
            foreach (var enemy in sharedData.EnemyConfigs)
            {
                for(var i = 0; i < enemy.Count; i++)
                {
                    var enemyEntity = world.NewEntity();

                    var enemyPool = world.GetPool<EnemyTagComponent>();
                    enemyPool.Add(enemyEntity);

                    var directionPool = world.GetPool<DirectionComponent>();
                    directionPool.Add(enemyEntity);
                    
                    var damagePool = world.GetPool<DamageComponent>();
                    damagePool.Add(enemyEntity);
                    
                    var HPPool = world.GetPool<HPComponent>();
                    HPPool.Add(enemyEntity);
                    ref var enemyHP = ref HPPool.Get(enemyEntity);

                    var speedPool = world.GetPool<SpeedComponent>();
                    speedPool.Add(enemyEntity);
                    ref var speedComponent = ref speedPool.Get(enemyEntity);

                    var animatorPool = world.GetPool<AnimatorComponent>();
                    animatorPool.Add(enemyEntity);
                    ref var animatorComponent = ref animatorPool.Get(enemyEntity);

                    var statePool = world.GetPool<EnemyStateComponent>();
                    statePool.Add(enemyEntity);
                    ref var stateComponent = ref statePool.Get(enemyEntity);


                    var enemyGO = GameObject.Instantiate(enemy.Prefab,
                                                        enemy.SpawnPoint.transform.position 
                                                        + new Vector3(i,0,i),
                                                        Quaternion.LookRotation(Vector3.back));
                    animatorComponent.Animator=enemyGO.GetComponentInChildren<Animator>();
                    speedComponent.Speed = enemy.Speed;
                    stateComponent.State = EnemyState.Idle;
                    enemyHP.Health = enemy.Health;
                    var entityLink = enemyGO.AddComponent<EntityLink>();
                    entityLink.EnemyEntityId = enemyEntity;
                    var enemyHitCollider = enemyGO.AddComponent<EnemyHitCollider>();
                    enemyHitCollider.Initialize(world, sharedData);
                }
            }
        }

        public void Run(IEcsSystems systems)
        {
            //throw new System.NotImplementedException();
        }
    }
}