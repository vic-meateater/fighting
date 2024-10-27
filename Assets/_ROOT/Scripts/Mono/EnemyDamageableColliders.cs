using UnityEngine;

namespace Fighting
{
    public class EnemyDamageableColliders : MonoBehaviour
    {
        [field: SerializeField] public GameObject DamageableCollider { get; private set; }
    }
}
