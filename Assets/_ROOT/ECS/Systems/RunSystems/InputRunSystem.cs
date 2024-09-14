using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class InputRunSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<DirectionComponent>().Inc<SpeedComponent>().End();
            var dirPool = world.GetPool<DirectionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();

            foreach (var entity in filter)
            {
                ref var direction = ref dirPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);

                // Получаем ввод с клавиатуры
                float horizontal = Input.GetAxisRaw("Horizontal"); // Для A и D
                float vertical = Input.GetAxisRaw("Vertical");     // Для W и S
                Debug.Log($"horizontal {horizontal} : vertical {vertical}");

                if (Input.GetKeyDown(KeyCode.Alpha5))
                     speed.Speed += 1f;

                // Задаем направление движения персонажа
                direction.Direction = new Vector3(horizontal, 0, vertical).normalized;
            }
        }
    }
}