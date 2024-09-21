using Leopotam.EcsLite;
using UnityEngine.InputSystem;

/// <summary>
/// This system responsible for handling player action buttons
/// </summary>
namespace Fighting {
    sealed class InputProcessingInitRunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private InputAction _actionKeyA;
        private InputAction _actionKeyB;
        private InputAction _actionKeyC;
        private EcsFilter _playerFilter;
        private EcsPool<PlayerActionComponent> _actionPool;

        public void Init(IEcsSystems systems)
        {

            _world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();

            // Инициализация Input Actions
            var inputAsset = sharedData.InputActionAsset;
            _actionKeyA = inputAsset.FindAction("ActionKeyA");
            _actionKeyB = inputAsset.FindAction("ActionKeyB");
            _actionKeyC = inputAsset.FindAction("ActionKeyC");

            _actionKeyA.Enable();
            _actionKeyB.Enable();
            _actionKeyC.Enable();

            _playerFilter = _world.Filter<PlayerActionComponent>().End();
            _actionPool = _world.GetPool<PlayerActionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            // Ищем игрока
            foreach (var entity in _playerFilter)
            {
                ref var actionComponent = ref _actionPool.Get(entity);

                // Сбрасываем предыдущие состояния
                actionComponent.ActionKeyA = false;
                actionComponent.ActionKeyB = false;
                actionComponent.ActionKeyC = false;

                // Обновляем состояние в зависимости от нажатий
                if (_actionKeyA.triggered)
                {
                    actionComponent.ActionKeyA = true;
                }

                if (_actionKeyB.triggered)
                {
                    actionComponent.ActionKeyB = true;
                }

                if (_actionKeyC.triggered)
                {
                    actionComponent.ActionKeyC = true;
                }
            }
        }
    }
}