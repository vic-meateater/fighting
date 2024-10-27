using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Fighting/Configs/EnemyConfig")]
    public class EnemyConfigSO : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject SpawnPoint;
        public float Speed;
        public int Count;
        public float Health;
    }
}
