using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// This system is responsible for react player actions
/// </summary>
namespace Fighting {
    sealed class PlayerActionRunSystem : IEcsRunSystem 
    {        
        public void Run (IEcsSystems systems) 
        {
            
            var world = systems.GetWorld();

            var filter = world.Filter<PlayerActionComponent>().End();
            var playerActionPool = world.GetPool<PlayerActionComponent>();

            foreach (var entity in filter)
            {
                ref var playerAction = ref playerActionPool.Get(entity);

                if (playerAction.ActionKeyA)
                    Debug.Log("PlayerAction A Pressed");

                if (playerAction.ActionKeyB)
                    Debug.Log("PlayerAction B Pressed");

                if (playerAction.ActionKeyC)
                    Debug.Log("PlayerAction C Pressed");
            }
        }
    }
}