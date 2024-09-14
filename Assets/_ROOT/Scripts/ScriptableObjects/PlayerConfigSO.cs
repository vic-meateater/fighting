using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Fighting/PlayerConfig")]
    public class PlayerConfigSO : ScriptableObject
    {
        public GameObject Prefab;
        public GameObject SpawnPoint;
        public float Speed;
    }
}
