using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting
{
    public class EnemyHitCollider : MonoBehaviour
    {
        private EcsWorld _world;
        private SharedData _sharedData;
        private int _playerDamage;

        // Передача ECS мира в PunchCollider, например, при инициализации
        public void Initialize(EcsWorld world, SharedData sharedData)
        {
            _world = world;
            _sharedData = sharedData;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerHitCollider")
            {
                var entityLink = GetComponent<EntityLink>();
                if (!entityLink) return;
                
                var enemyEntity = entityLink.EnemyEntityId;
                var playerEntity = _sharedData.PlayerConfig.EntityID;

                //var filter =_world.Filter<DamageComponent>().Inc<EnemyTagComponent>().End();
                var damagePool = _world.GetPool<DamageComponent>();
                var enemyTagPool = _world.GetPool<EnemyTagComponent>();
                var playerStatePool = _world.GetPool<PlayerStateComponent>();
                
                // ref var playerState = ref playerStatePool.Get(playerEntity);
                // foreach (var entity in filter)
                // {
                //     if (entity == enemyEntity)
                //     {
                //         ref var damage = ref damagePool.Get(enemyEntity);
                //         damage.IsHit = true;
                //         damage.DamageAmount = 0; // Устанавливаем количество урона
                //         switch (playerState.State)
                //         {
                //             case PlayerState.AttackA:
                //                 _playerDamage = _sharedData.PlayerConfig.SelectedSkills[0].DamageAmount;
                //                 damage.DamageAmount = _playerDamage;
                //                 break;
                //             case PlayerState.AttackB:
                //                 _playerDamage = _sharedData.PlayerConfig.SelectedSkills[1].DamageAmount;
                //                 damage.DamageAmount = _playerDamage;
                //                 break;
                //             case PlayerState.AttackC:
                //                 _playerDamage = _sharedData.PlayerConfig.SelectedSkills[2].DamageAmount;
                //                 damage.DamageAmount = _playerDamage;
                //                 break;
                //             default:
                //                 _playerDamage = 0;
                //                 break;
                //         }
                //     }
                // }
                if (damagePool.Has(enemyEntity) && enemyTagPool.Has(enemyEntity))
                {
                    ref var damage = ref damagePool.Get(enemyEntity);
                    ref var playerState = ref playerStatePool.Get(playerEntity);

                    damage.IsHit = true;

                    // Устанавливаем урон в зависимости от состояния игрока
                    _playerDamage = playerState.State switch
                    {
                        PlayerState.AttackA => _sharedData.PlayerConfig.SelectedSkills[0].DamageAmount,
                        PlayerState.AttackB => _sharedData.PlayerConfig.SelectedSkills[1].DamageAmount,
                        PlayerState.AttackC => _sharedData.PlayerConfig.SelectedSkills[2].DamageAmount,
                        _ => 0
                    };

                    damage.DamageAmount = _playerDamage;
                }
            }
        }
    }
}
