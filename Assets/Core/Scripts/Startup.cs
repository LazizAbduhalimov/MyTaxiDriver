using Leopotam.EcsLite;
using UnityEngine;
using AB_Utility.FromSceneToEntityConverter;
using Client.Game.Test;
using Leopotam.EcsLite.Di;
using SevenBoldPencil.EasyEvents;

namespace Client {
    public sealed class Startup : MonoBehaviour 
    {
        private EcsWorld _world;        
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _initSystems;
        private EventsBus _eventsBus;

        private void Start () 
        {
            _eventsBus = new EventsBus();

            _world = new EcsWorld ();
            _initSystems = new EcsSystems(_world);
            _updateSystems = new EcsSystems (_world);
            _fixedUpdateSystems = new EcsSystems(_world);
            
            AddInitSystems();
            AddRunSystems();
            AddEditorSystems();

            InjectAllSystems(_initSystems, _updateSystems, _fixedUpdateSystems);
            AddEventsDestroyer();
            
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
                .Add(new InitTaxiCoords())
                ;
        }
        
        private void AddRunSystems() 
        {
            _updateSystems
                .Add(new DragAndDropSystem())
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

            _eventsBus.Destroy();
        }

        private void AddEditorSystems() 
        {
            // #if UNITY_EDITOR
            //     _updateSystems
            //         .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ());
            // #endif
        }

        private void AddEventsDestroyer()
        {
            _updateSystems
                .Add(_eventsBus.GetDestroyEventsSystem()
                )
                ;
        }

        private void InjectAllSystems(params IEcsSystems[] systems)
        {
            foreach (var system in systems)
            {
                system.Inject(_eventsBus);
            }
        }
    }
}