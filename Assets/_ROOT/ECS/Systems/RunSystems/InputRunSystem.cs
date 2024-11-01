using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This system is responsible for handling player movement
/// </summary>
namespace Fighting {
    sealed class InputRunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private InputAction moveAction;

        public void Init(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            
            var inputAsset = sharedData.InputActionAsset;
            moveAction = inputAsset.FindAction("Move");

            if (moveAction != null)
            {
                moveAction.Enable();
            }
            else
            {
                Debug.LogError("Move action not found in InputActionAsset!");
            }
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<DirectionComponent>()
                                        .Inc<SpeedComponent>()
                                        .Inc<PlayerActionComponent>()
                                        .Inc<PlayerTagComponent>().End();

            var dirPool = world.GetPool<DirectionComponent>();
            var playerActionPool = world.GetPool<PlayerActionComponent>();

            foreach (var entity in filter)
            {
                ref var playerAction = ref playerActionPool.Get(entity);
                if (playerAction.IsControlBlocked) 
                    continue;

                ref var direction = ref dirPool.Get(entity);
                
                Vector2 moveInput = moveAction.ReadValue<Vector2>();
                direction.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            }
        }
    }
}