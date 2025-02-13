using Leopotam.EcsLite;
using UnityEngine;
using AB_Utility.FromSceneToEntityConverter;
using Client.Game;
using Client.Game.Test;
using Client.Saving;
using Game;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using LGrid;
using LSound;
using Module;
using Module.Bank;
using Module.Systems;
using PoolSystem.Alternative;
using UI;

namespace Client {
    public sealed class TextStartup : MonoBehaviour
    {
        private EcsWorld _world, _eventsWorld;        
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _initSystems;

        private void Start () 
        {
            _world = new EcsWorld ();
            _eventsWorld = new EcsWorld();
            _initSystems = new EcsSystems(_world);
            _updateSystems = new EcsSystems (_world);
            _fixedUpdateSystems = new EcsSystems(_world);

            CommonUtilities.Init(_world, _eventsWorld);
            Bank.EventsWorld = _eventsWorld;
            
            AddInitSystems();
            AddRunSystems();
            AddEditorSystems();

            
            _initSystems.Inject().Init();
            _fixedUpdateSystems.Inject().Init();
            _updateSystems.Inject().Init();
        }

        private void Update () 
        {
            _updateSystems?.Run ();
        }
        
        private void FixedUpdate() 
        {
            _fixedUpdateSystems?.Run();
        }

        private void AddInitSystems()
        {
            _initSystems
                .AddWorld(_eventsWorld, "events")
                ;
        }
        
        private void AddRunSystems() 
        {
            _updateSystems
                .AddWorld(_eventsWorld, "events")
                .Add(new ChangeObjectViewUsageExampleSystem())
                .Add(new ChangeMeshAndTextureSystem())
                ;
        }

        private void OnDestroy () 
        {
            _updateSystems?.Destroy ();
            _updateSystems = null;

            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;

            _world?.Destroy ();
            _world = null;
        }

        private void AddEditorSystems() 
        {
            // #if UNITY_EDITOR
            //     _updateSystems
            //         .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ());
            // #endif
        }
    }
}