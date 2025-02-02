using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine.SceneManagement;

namespace Client.Saving
{
    public class DropDataSystem : IEcsRunSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsFilterInject<Inc<CSaveFileSetup>> _cSaveFileSetupFilter;
        private EcsFilterInject<Inc<EDropData>> _eDropData = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eDropData.Value) DropData(entity);
        }

        private void DropData(int eventEntity)
        {
            _map.Value.Clear();
            foreach (var entity in _cSaveFileSetupFilter.Value)
            {
                ref var setupFile = ref _cSaveFileSetupFilter.Pools.Inc1.Get(entity);
                var file = setupFile.SaveFileSetupMb.File.GetSaveFile();
                file.DeleteData("PurchaseNumber");
                file.DeleteData("Coins");
                file.Save();
                SceneManager.LoadScene(0);   
            }
            _eDropData.Pools.Inc1.Del(eventEntity);
        }
    }
}