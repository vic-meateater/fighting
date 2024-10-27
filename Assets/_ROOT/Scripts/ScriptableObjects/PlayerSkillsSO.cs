using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fighting
{
    [CreateAssetMenu(fileName = "PlayerSkillsConfig", menuName = "Fighting/Configs/PlayerSkillsConfig")]
    public class PlayerSkillsSO : ScriptableObject
    {
        [System.Serializable]
        public struct Skill
        {
            public string PlayerAnimatorTrigger;        // Название скилла
            public string EnemyAnimatorTrigger;  // Триггер для аниматора
            public int DamageAmount;        // Количество урона
        }

        // Список всех доступных скиллов
        public List<Skill> AvailableSkills = new List<Skill>
        {
            new Skill { PlayerAnimatorTrigger = "Hook", EnemyAnimatorTrigger = "Hook", DamageAmount = 10 },
            new Skill { PlayerAnimatorTrigger = "Kick", EnemyAnimatorTrigger = "Kick", DamageAmount = 15 },
            new Skill { PlayerAnimatorTrigger = "Combo", EnemyAnimatorTrigger = "Combo", DamageAmount = 25 }
        };
    }
}
