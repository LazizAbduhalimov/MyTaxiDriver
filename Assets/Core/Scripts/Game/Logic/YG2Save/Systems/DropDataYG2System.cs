using Game.Saving;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using LGrid;
using UnityEngine.SceneManagement;

namespace YG.Systems
{
    public class DropDataYG2System : IEcsRunSystem
    {
        private EcsCustomInject<Map> _map;
        private EcsFilterInject<Inc<EDropData>> _eDropData = "events";
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eDropData.Value) DropData(entity);
        }

        private void DropData(int eventEntity)
        {
            _map.Value.Clear();
            YG2.SetDefaultSaves();
            _eDropData.Pools.Inc1.Del(eventEntity);
            SceneManager.LoadScene(0);   
        }
    }
}