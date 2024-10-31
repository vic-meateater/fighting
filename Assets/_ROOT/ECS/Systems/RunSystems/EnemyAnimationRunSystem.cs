using System;
using Leopotam.EcsLite;

namespace Fighting {
    sealed class EnemyAnimationRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            
            var filter = world.Filter<AnimatorComponent>()
                                 .Inc<EnemyTagComponent>()
                                 .Inc<EnemyStateComponent>()
                                 .Inc<SpeedComponent>()
                                 .End();
            
            var animatorPool = world.GetPool<AnimatorComponent>();
            var statePool = world.GetPool<EnemyStateComponent>();
            var actionPool = world.GetPool<EnemyActionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            
            //Triggers for animator controller from PlayerConfigSO
            var damageA = sharedData.PlayerConfig.SelectedSkills[0].EnemyAnimatorTrigger;
            var damageB = sharedData.PlayerConfig.SelectedSkills[1].EnemyAnimatorTrigger;
            var damageC = sharedData.PlayerConfig.SelectedSkills[2].EnemyAnimatorTrigger;
            
            foreach (var entity in filter)
            {
                ref var animatorComponent = ref animatorPool.Get(entity);
                var animator = animatorComponent.Animator;
                ref var enemyState = ref statePool.Get(entity);
                ref var enemySpeed = ref speedPool.Get(entity);
                
                animator.SetFloat("Speed", enemySpeed.Speed);

                switch (enemyState.State)
                {
                    case EnemyState.Idle:
                        break;
                    case EnemyState.Attack:
                        break;
                    case EnemyState.TakingDamageA:
                        animator.SetTrigger(damageA);
                        enemyState.State = EnemyState.Idle;
                        break;
                    case EnemyState.TakingDamageB:
                        animator.SetTrigger(damageB);
                        enemyState.State = EnemyState.Idle;
                        break;
                    case EnemyState.TakingDamageC:
                        animator.SetTrigger(damageC);
                        enemyState.State = EnemyState.Idle;
                        break;
                    case EnemyState.StandUP:
                        break;
                    case EnemyState.Following:
                        break;
                    case EnemyState.Die:
                        animator.SetTrigger("Die");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}