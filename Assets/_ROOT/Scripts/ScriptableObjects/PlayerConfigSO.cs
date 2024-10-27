using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Fighting/Configs/PlayerConfig")]
    [System.Serializable]
    public class PlayerConfigSO : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject SpawnPoint;
        public float Speed;
        public float Health;
        public int EntityID;
        
        [Tooltip("Список всех доступных скиллов")]
        public PlayerSkillsSO SkillsConfig;

        [Tooltip("Выбранные скиллы для персонажа")]
        public List<string> SelectedSkillNames = new List<string>(3);  // Только имена скиллов

        public List<PlayerSkillsSO.Skill> SelectedSkills = new List<PlayerSkillsSO.Skill>(3);

        private void OnValidate()
        {
            if (SkillsConfig != null && SelectedSkillNames != null)
            {
                SelectedSkills.Clear();
                foreach (var skillName in SelectedSkillNames)
                {
                    var skill = SkillsConfig.AvailableSkills.Find(s => s.PlayerAnimatorTrigger == skillName);
                    if (skill.PlayerAnimatorTrigger != null)
                    {
                        SelectedSkills.Add(skill);
                    }
                }
            }
        }
    }
}
