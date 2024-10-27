using Leopotam.EcsLite;

namespace Fighting {
    sealed class EnemyDamageRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<DamageComponent>().Inc<EnemyStateComponent>().Inc<HPComponent>().End();

            var damagePool = world.GetPool<DamageComponent>();
            var statePool = world.GetPool<EnemyStateComponent>();
            var HPPool = world.GetPool<HPComponent>();
            var playerStatePool = world.GetPool<PlayerStateComponent>();
            ref var playerState = ref playerStatePool.Get(sharedData.PlayerConfig.EntityID);

            foreach (var entity in filter)
            {
                ref var damage = ref damagePool.Get(entity);
                ref var enemyState = ref statePool.Get(entity);
                ref var enemyHP = ref HPPool.Get(entity);

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
                    
                    //enemyState.State = EnemyState.TakingDamageA;
                    // Сбрасываем флаг
                    damage.IsHit = false;
                    enemyHP.Health -= damage.DamageAmount;
                }
            }
        }
    }
}