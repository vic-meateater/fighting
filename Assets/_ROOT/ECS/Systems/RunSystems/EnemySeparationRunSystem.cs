using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class EnemySeparationRunSystem : IEcsRunSystem {        
        public void Run (IEcsSystems systems) {
             var world = systems.GetWorld();

            // Фильтр для врагов
            var enemyFilter = world.Filter<EnemyTagComponent>()
                                    .Inc<PositionComponent>()
                                    .Inc<SpeedComponent>()
                                    .End();
            
            var positionPool = world.GetPool<PositionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();

            float separationDistance = 1.5f; // Минимальная дистанция между врагами
            float separationStrength = 1f; // Коэффициент силы отталкивания

            // Пройтись по каждому врагу в фильтре
            foreach (var enemyEntity in enemyFilter) {
                ref var enemyPosition = ref positionPool.Get(enemyEntity);
                ref var enemySpeed = ref speedPool.Get(enemyEntity);

                Vector3 separationForce = Vector3.zero; // Сила отталкивания от других врагов

                // Пройтись по другим врагам, чтобы вычислить силу отталкивания
                foreach (var otherEnemyEntity in enemyFilter) {
                    if (enemyEntity == otherEnemyEntity) continue; // Пропустить самого себя
                    
                    ref var otherEnemyPosition = ref positionPool.Get(otherEnemyEntity);

                    // Вычислить дистанцию до другого врага
                    float distanceToOther = Vector3.Distance(enemyPosition.Position, otherEnemyPosition.Position);

                    // Если враги слишком близко, применить силу отталкивания
                    if (distanceToOther < separationDistance) {
                        Vector3 directionAwayFromOther = (enemyPosition.Position - otherEnemyPosition.Position).normalized;
                        float strength = (separationDistance - distanceToOther) / separationDistance * separationStrength;
                        separationForce += directionAwayFromOther * strength;
                    }
                }

                // Применить силу отталкивания к позиции врага
                enemyPosition.Position += separationForce * Time.deltaTime;

                // Дополнительно можно синхронизировать позицию объекта в Unity
                var enemyGO = world.GetPool<GameObjectComponent>().Get(enemyEntity).gameObject;
                if (enemyGO) {
                    enemyGO.transform.position = enemyPosition.Position;
                }
            }
        }
    }
}