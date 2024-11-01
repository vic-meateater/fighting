using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

namespace Fighting {
    sealed class EnemyDieRunSystem : IEcsRunSystem 
    {        
        public void Run (IEcsSystems systems) 
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<EnemyDieComponent>().Inc<AnimatorComponent>().Inc<GameObjectComponent>().End();

            var enemyDiePool = world.GetPool<EnemyDieComponent>();
            var animatorPool = world.GetPool<AnimatorComponent>();
            var goPool = world.GetPool<GameObjectComponent>();

            foreach (var entity in filter)
            {
                ref var animator = ref animatorPool.Get(entity);
                ref var enemy = ref enemyDiePool.Get(entity);
                ref var go = ref goPool.Get(entity); 
                
                // Проверяем, находится ли анимация на последнем кадре клипа смерти
                var stateInfo = animator.Animator.GetCurrentAnimatorStateInfo(0);
                
                //Отключаем коллайдер для урона
                //go.gameObject.GetComponent<BoxCollider>().enabled = false;
                
                if (stateInfo.normalizedTime >= 1.2f) {
                    
                    // Удаление объекта из сцены, если он еще существует
                    //var go = animator.Animator.GetComponentInParent<Rigidbody>().gameObject;
                    //var go 
                    if (go.gameObject) {
                        
                        GameObject.Destroy(go.gameObject);
                    }
                    world.DelEntity(entity);
                }
            }
        }
    }
}