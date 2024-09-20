using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fighting {
    sealed class InputRunSystem : IEcsRunSystem
    {
        private InputAction moveAction;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var filter = world.Filter<DirectionComponent>().Inc<SpeedComponent>().End();
            var dirPool = world.GetPool<DirectionComponent>();
            var speedPool = world.GetPool<SpeedComponent>();

            var inputAsset = sharedData.inputActionAsset;
            moveAction = inputAsset.FindAction("Move");

            moveAction.Enable();


            foreach (var entity in filter)
            {
                ref var direction = ref dirPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);

                // �������� ���� � ����������
                //float horizontal = Input.GetAxisRaw("Horizontal"); // ��� A � D
                //float vertical = Input.GetAxisRaw("Vertical");     // ��� W � S
                //Debug.Log($"horizontal {horizontal} : vertical {vertical}");

                Vector2 moveInput = moveAction.ReadValue<Vector2>(); // ������ �������� ��� ���� X � Y
                Debug.Log($"horizontal {moveInput.x} : vertical {moveInput.y}");

                //if (Input.GetKeyDown(KeyCode.Alpha5))
                //     speed.Speed += 1f;

                // ������ ����������� �������� ���������
                //direction.Direction = new Vector3(horizontal, 0, vertical).normalized;
                direction.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            }
        }
    }
}