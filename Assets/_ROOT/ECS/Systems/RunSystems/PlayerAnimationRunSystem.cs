using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class PlayerAnimationRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
            var world = systems.GetWorld();
            var filter = world.Filter<DirectionComponent>().Inc<SpeedComponent>().Inc<AnimatorComponent>().End();

            var dirPool = world.GetPool<DirectionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            var animatorPool = world.GetPool<AnimatorComponent>();

            foreach (var entity in filter)
            {
                ref var direction = ref dirPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);
                ref var animatorComponent = ref animatorPool.Get(entity);

                var animator = animatorComponent.Animator;

                if (animator != null)
                {
                    animator.SetFloat("Speed", speed.Speed * direction.Direction.magnitude);
                }
            }
        }
    }
}