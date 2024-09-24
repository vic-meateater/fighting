using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This system is responsible for react player actions
/// </summary>
namespace Fighting {
    sealed class PlayerActionRunSystem : IEcsRunSystem 
    {        
        public void Run (IEcsSystems systems) 
        {
            
            var world = systems.GetWorld();

            var filter = world.Filter<PlayerActionComponent>()
                                      .Inc<AnimatorComponent>()
                                      .Inc<PlayerTagComponent>()
                                      .Inc<SpeedComponent>()
                                      .End();
            var playerActionPool = world.GetPool<PlayerActionComponent>();
            var animatorPool = world.GetPool<AnimatorComponent>();
            var speedPool = world.GetPool<SpeedComponent>();

            foreach (var entity in filter)
            {
                ref var playerAction = ref playerActionPool.Get(entity);
                ref var animator = ref animatorPool.Get(entity);
                ref var speed = ref speedPool.Get(entity);

                var tmpSpeed = speed.Speed;
                // ���������, ������������� �� ����������
                if (playerAction.IsControlBlocked)
                {
                    // ���� ���������� �������������, ���������, ����������� �� ��������
                    var stateInfo = animator.Animator.GetCurrentAnimatorStateInfo(0);
                    // �������� ���������� � ������� ���������
                    
                    speed.Speed = 0;
                    var currentClipLenght = CurrentClipLength(animator.Animator);

                    //need refactor this
                    if (stateInfo.normalizedTime>= 0.17f && !animator.Animator.IsInTransition(0))
                    {
                        playerAction.IsControlBlocked = false;
                        speed.Speed = 2;
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

        private float CurrentClipLength(Animator animator)
        {
            var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            
            if (currentClipInfo.Length > 0) // ���������, ��� ���� �������� ����� �� ������� ����
            {
                AnimationClip currentClip = currentClipInfo[0].clip; // ����� ������ (� ������ ������������) ����
                float currentClipLength = currentClip.length; // ����� �������� �����

                Debug.Log($"����� �������� �������������� �����: {currentClipLength} ������");
                return currentClipLength;
            }
            else
            {
                Debug.LogWarning("��� ��������� ����� �� ������ ����!");
                return 0;
            }
        }
    }
}