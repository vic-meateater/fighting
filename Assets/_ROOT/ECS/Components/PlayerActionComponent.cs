using UnityEngine;

namespace Fighting {
    struct PlayerActionComponent 
    {
        public bool ActionKeyA;
        public bool ActionKeyB;
        public bool ActionKeyC;
        public bool IsControlBlocked;
        public GameObject LeftArm;
        public GameObject RightArm;
        public GameObject LeftLeg;
        public GameObject RightLeg;
    }
}