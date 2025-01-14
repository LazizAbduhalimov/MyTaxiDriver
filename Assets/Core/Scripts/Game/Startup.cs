using Leopotam.EcsLite;
using UnityEngine;
using AB_Utility.FromSceneToEntityConverter;
using Client.Game;
using Client.Game.Test;
using Core.Scripts.Game;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Module.Bank;

namespace Client {
    public sealed class Startup : MonoBehaviour
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

            Utilities.World = _world;
            Utilities.EventsWorld = _eventsWorld;
            
            AddInitSystems();
            AddRunSystems();
            AddEditorSystems();

            InjectAllSystems(_initSystems, _updateSystems, _fixedUpdateSystems);
            
            _initSystems.Init();
            _fixedUpdateSystems.Init();
            _updateSystems.ConvertScene().Init();
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
                .Add(new MapInitSystem())
                .Add(new SettingsSystem())
                .Add(new InitTaxiCoords())
                ;
        }
        
        private void AddRunSystems() 
        {
            _updateSystems
                .AddWorld(_eventsWorld, "events")
                .Add(new DragAndDropSystem())
                
                .Add(new EarnMoneySystem())
                .Add(new BankSystem())
                
                .DelHere<EEarnMoney>("events")
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

        private void InjectAllSystems(params IEcsSystems[] systems)
        {
            var premadePools = FindObjectOfType<AllPools>();
            foreach (var system in systems)
            {
                system.Inject(premadePools)
                    ;
            }
        }
    }
}