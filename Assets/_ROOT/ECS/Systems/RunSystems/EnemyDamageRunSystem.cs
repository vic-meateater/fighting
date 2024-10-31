using System;
using Leopotam.EcsLite;
using Microlight.MicroBar;
using UnityEngine;

namespace Fighting {
    sealed class EnemyDamageRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<DamageComponent>()
                                 .Inc<EnemyStateComponent>()
                                 .Inc<HPComponent>()
                                 .Inc<GameObjectComponent>()
                                 .End();

            var damagePool = world.GetPool<DamageComponent>();
            var statePool = world.GetPool<EnemyStateComponent>();
            var HPPool = world.GetPool<HPComponent>();
            var playerStatePool = world.GetPool<PlayerStateComponent>();
            var goPool = world.GetPool<GameObjectComponent>();
            
            ref var playerState = ref playerStatePool.Get(sharedData.PlayerConfig.EntityID);

            foreach (var entity in filter)
            {
                ref var damage = ref damagePool.Get(entity);
                ref var enemyState = ref statePool.Get(entity);
                ref var enemyHP = ref HPPool.Get(entity);
                ref var go = ref goPool.Get(entity);

                if (damage.IsHit)
                {
                    // Проигрываем анимацию урона
                    enemyState.State = playerState.State switch
                    {
                        PlayerState.AttackA => EnemyState.TakingDamageA,
                        PlayerState.AttackB => EnemyState.TakingDamageB,
                        PlayerState.AttackC => EnemyState.TakingDamageC,
                        _ => EnemyState.Idle
                    };

                    damage.IsHit = false;
                    enemyHP.CurrentHealth -= damage.DamageAmount;
                    if (enemyHP.CurrentHealth > 0)
                    {
                        enemyHP.MicroBar.UpdateBar(enemyHP.CurrentHealth, UpdateAnim.Damage);
                    }
                    else
                    {
                        enemyHP.CurrentHealth = 0;
                        enemyHP.MicroBar.UpdateBar(enemyHP.CurrentHealth, UpdateAnim.Damage);
                        enemyState.State = EnemyState.Die;
                        var enemyDiePool = world.GetPool<EnemyDieComponent>();
                        enemyDiePool.Add(entity);
                    }

                }

                // go.gameObject.GetComponent<BoxCollider>().enabled = enemyState.State switch
                // {
                //     EnemyState.Idle => true,
                //     EnemyState.Attack => true,
                //     EnemyState.TakingDamageA => false,
                //     EnemyState.TakingDamageB => false,
                //     EnemyState.TakingDamageC => false,
                //     EnemyState.StandUP => true,
                //     EnemyState.Following => true,
                //     EnemyState.Die => false,
                //     _ => throw new ArgumentOutOfRangeException()
                // };
            }
        }
    }
}