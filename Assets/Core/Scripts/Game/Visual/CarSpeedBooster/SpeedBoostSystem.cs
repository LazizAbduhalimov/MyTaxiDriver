using Client.Game;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using PrimeTween;
using UnityEngine;

namespace Client
{
    public class SpeedBoostSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EBoostSpeed>> _eSpeedBoostFilter = "events";

        private EcsPoolInject<CTaxi> _cTaxi;
        private EcsPoolInject<CSpeedBooster> _cSpeedBooster;
         
        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _eSpeedBoostFilter.Value)
            {
                ref var boostData = ref _eSpeedBoostFilter.Pools.Inc1.Get(eventEntity);
                var booster = boostData.SpeedBoosterMb;
                var entity = booster.PackedEntity.FastUnpack();
                Boost(entity, boostData.BoostTime);
            }    
        }

        private void Boost(int entity, float duration)
        {
            ref var cBooster = ref _cSpeedBooster.Value.Get(entity);
            var taxiMb = _cTaxi.Value.Get(entity).TaxiMb;
            var booster = cBooster.SpeedBoosterMb;
            if (!booster.CanBeBoosted)
            {
                booster.BoostSequence?.Complete();
                booster.BoostSequence = null;
            }
            var startSpeed = taxiMb.Follower.speed;
            var r = Random.Range(0.8f, 1.1f); 
            var boostedSpeed = startSpeed + startSpeed * booster.BoostPercentage * r;
            SetBoostState(booster, true);
            booster.BoostSequence?.Complete();
            booster.BoostSequence = Sequence.Create(cycles: 2, CycleMode.Yoyo, Ease.OutSine)
                    .Chain(Tween.Custom(startSpeed, boostedSpeed, duration: duration * 0.15f, 
                        value => taxiMb.Follower.speed = value))
                    .Chain(Tween.Delay(duration * 0.70f))
                    .OnComplete(() => {
                        Tween.Delay(booster.BoostCoolDown, () => SetBoostState(booster, false));
                    });
        }

        private static void SetBoostState(SpeedBoosterMb booster, bool isBoosting)
        {
            if (!isBoosting && booster.BoostSequence is { isAlive: true })
                return;
            booster.Trails.gameObject.SetActive(isBoosting);
            booster.CanBeBoosted = !isBoosting;
            booster.Particle.SetActive(!isBoosting);
            if (!isBoosting) Debug.Log("Completed");
        }
    }
}