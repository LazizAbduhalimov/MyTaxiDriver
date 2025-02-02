using Leopotam.EcsLite;
using UnityEngine;
using AB_Utility.FromSceneToEntityConverter;
using Client.Game;
using Client.Game.Test;
using Game;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using LGrid;
using LSound;
using Module.Bank;
using PoolSystem.Alternative;
using UI;

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

            Utilities.Init(_world, _eventsWorld);
            Bank.EventsWorld = _eventsWorld;
            
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
                .Add(new SettingsSystem())
                .Add(new MapInitSystem())
                .Add(new GridInitSystem())
                .Add(new CarsInitSystem())
                .Add(new InitCarsCoords())
                .Add(new InitUIInterface())
                .Add(new InitUIButtons())
                ;
        }
        
        private void AddRunSystems() 
        {
            _updateSystems
                .AddWorld(_eventsWorld, "events")
                .Add(new DragAndDropMarkerSystem())
                .Add(new DragHandleSystem())
                .Add(new EarnMoneySystem())
                
                .Add(new VehiclePurchaseSystem())
                .Add(new VehiclePurchaseButtonStateHandleSystem())
                
                .Add(new CoinDisplaySystem())
                
                .Add(new SoundBridgeSystem())
                .Add(new MusicBridgeSystem())
                .Add(new SoundSystem())
                .Add(new MusicSystem())
                
                .DelHere<EEarnMoney>("events")
                .DelHere<EBankValueChanged>("events")
                
                .DelHere<EDragStart>("events")
                .DelHere<EDragEnd>("events")
                .AddUIEventsDestroyers()
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
            var map = new Map();
            var premadePools = FindObjectOfType<AllPools>();
            var poolService = new PoolService("Pools");
            var gameData = new GameData();
            foreach (var system in systems)
            {
                system.Inject(premadePools)
                      .Inject(gameData)
                      .Inject(poolService)
                      .Inject(map)
                    ;
            }
        }
    }
}