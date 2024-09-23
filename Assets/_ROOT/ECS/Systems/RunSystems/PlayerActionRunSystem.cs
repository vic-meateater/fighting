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

            var filter = world.Filter<PlayerActionComponent>().Inc<AnimatorComponent>().Inc<PlayerTagComponent>().End();
            var playerActionPool = world.GetPool<PlayerActionComponent>();
            var animatorPool = world.GetPool<AnimatorComponent>();

            foreach (var entity in filter)
            {
                ref var playerAction = ref playerActionPool.Get(entity);
                ref var animator = ref animatorPool.Get(entity);
                
                // ���������, ������������� �� ����������
                if (playerAction.IsControlBlocked)
                {
                    // ���� ���������� �������������, ���������, ����������� �� ��������
                    var stateInfo = animator.Animator.GetCurrentAnimatorStateInfo(0); // �������� ���������� � ������� ���������
                    if (!stateInfo.IsTag("Attack")) // ���������, ��� ��� �������� ����� ����������� ��������������� �������� tag
                    {
                        Debug.Log(stateInfo.ToString());
                        playerAction.IsControlBlocked = false; // ������������ ����������, ���� �������� �����������
                    }
                    continue; // ���������� ���������� ��������� �����
                }

                if (playerAction.ActionKeyA)
                {
                    animator.Animator.SetTrigger("Hook");
                    playerAction.IsControlBlocked = true;
                }

                if (playerAction.ActionKeyB)
                    Debug.Log("PlayerAction B Pressed");

                if (playerAction.ActionKeyC)
                    Debug.Log("PlayerAction C Pressed");
            }
        }
    }
}