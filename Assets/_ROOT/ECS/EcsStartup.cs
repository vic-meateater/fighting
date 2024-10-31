using Leopotam.EcsLite;
using UnityEngine;

namespace Fighting {
    sealed class EcsStartup : MonoBehaviour {

        [SerializeField] SharedData _sharedData;
        
        EcsWorld _world;        
        IEcsSystems _systems;

        void Start () {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world, _sharedData);
            _systems
                .Add(new PlayerInitSystem())
                .Add(new EnemySpawnInitRunSystem())
                .Add(new InputProcessingInitRunSystem())
                .Add(new PlayerActionRunSystem())
                .Add(new InputRunSystem())
                .Add(new PlayerAnimationRunSystem())
                .Add(new EnemyAnimationRunSystem())
                .Add(new MovementRunSystem())
                .Add(new EnemyDamageRunSystem())
                .Add(new EnemyFollowRunSystem())
                .Add(new EnemySeparationRunSystem())
                .Add(new EnemyDieRunSystem())


                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();
        }

        void Update () {
            // process systems here.
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy ();
                _systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }
    }
}