using UnityEngine;

namespace Fighting
{
    public class PlayerDamageableColliders : MonoBehaviour
    {
        [field: SerializeField] public GameObject LeftArm { get; private set; }
        [field: SerializeField] public GameObject RightArm { get; private set; }
        [field: SerializeField] public GameObject LeftLeg { get; private set; }
        [field: SerializeField] public GameObject RightLeg { get; private set; }
    }
}
