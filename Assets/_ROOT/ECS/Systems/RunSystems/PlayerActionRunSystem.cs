using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// This system is responsible for react player actions
/// </summary>
namespace Fighting {
    sealed class PlayerActionRunSystem : IEcsRunSystem 
    {
        private const string IDLE_STATE_NAME = "Idle";

        public void Run (IEcsSystems systems) 
        {
            
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            
            var filter = world.Filter<PlayerActionComponent>()
                                      .Inc<AnimatorComponent>()
                                      .Inc<PlayerTagComponent>()
                                      .Inc<SpeedComponent>()
                                      .Inc<PlayerStateComponent>()
                                      .End();
            var playerActionPool = world.GetPool<PlayerActionComponent>();
            var animatorPool = world.GetPool<AnimatorComponent>();
            var speedPool = world.GetPool<SpeedComponent>();
            var playerStatePool = world.GetPool<PlayerStateComponent>();

            foreach (var entity in filter)
            {
                ref var playerAction = ref playerActionPool.Get(entity);
                ref var animator = ref animatorPool.Get(entity);
                ref var playerState = ref playerStatePool.Get(entity);
                ref var speed = ref speedPool.Get(entity);
                
                if (playerAction.IsControlBlocked)
                {
                    var stateInfo = animator.Animator.GetCurrentAnimatorStateInfo(0);
                    var clipInfo = animator.Animator.GetCurrentAnimatorClipInfo(0);

                    var currentClipLenght = CurrentClipLength(animator.Animator);

                    //need refactor this
                    if (stateInfo.IsName(IDLE_STATE_NAME) && !animator.Animator.IsInTransition(0))
                    {
                        playerState.State = PlayerState.Idle;
                        playerAction.RightArm.SetActive(false);
                        playerAction.LeftArm.SetActive(false);
                        playerAction.RightLeg.SetActive(false);
                        playerAction.LeftLeg.SetActive(false);
                        playerAction.IsControlBlocked = false;
                    }
                    continue;
                }

                if (playerAction.ActionKeyA)
                {
                    var animationTriggerParam = sharedData.PlayerConfig.SelectedSkills[0].PlayerAnimatorTrigger; 
                    animator.Animator.SetTrigger(animationTriggerParam);
                    playerAction.IsControlBlocked = true;
                    playerAction.RightArm.SetActive(playerAction.IsControlBlocked);
                    playerState.State = PlayerState.AttackA;
                }

                if (playerAction.ActionKeyB)
                {
                    var animationTriggerParam = sharedData.PlayerConfig.SelectedSkills[1].PlayerAnimatorTrigger;
                    animator.Animator.SetTrigger(animationTriggerParam);
                    playerAction.IsControlBlocked = true;
                    playerAction.RightLeg.SetActive(playerAction.IsControlBlocked);
                    playerState.State = PlayerState.AttackB;
                }

                if (playerAction.ActionKeyC)
                {
                    var animationTriggerParam = sharedData.PlayerConfig.SelectedSkills[2].PlayerAnimatorTrigger;
                    animator.Animator.SetTrigger(animationTriggerParam);
                    playerAction.IsControlBlocked = true;
                    // playerAction.RightArm.SetActive(playerAction.IsControlBlocked);
                    // playerAction.LeftArm.SetActive(playerAction.IsControlBlocked);
                    playerAction.RightLeg.SetActive(playerAction.IsControlBlocked);
                    //playerAction.LeftLeg.SetActive(playerAction.IsControlBlocked);
                    //playerState.State = playerState.State == PlayerState.AttackC ? PlayerState.Idle : PlayerState.AttackC;
                    playerState.State = PlayerState.AttackC;
                }
            }
        }

        private float CurrentClipLength(Animator animator)
        {
            var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            
            if (currentClipInfo.Length > 0) 
            {
                AnimationClip currentClip = currentClipInfo[0].clip;
                float currentClipLength = currentClip.length;
                return currentClipLength;
            }
            else
            {
                return 0;
            }
        }
    }
}