using Client.Game.Test;
using Leopotam.EcsLite;
using PathCreation.Examples;
using UnityEngine;

namespace Client.Game
{
    public class TaxiMb : MonoBehaviour
    {
        public EcsPackedEntity PackedEntity;
        public int Level;
        public float Speed;
        public int MoneyForCircle;

        [HideInInspector] public PathFollower Follower;
        [HideInInspector] public Transform TransparentGfx;
        private Vector3 _defaultOffset;
        
        public void Init()
        {
            var world = CommonUtilities.World;
            var dragAndDrop = GetComponent<DragAndDropMb>();
            var speedBooster = GetComponentInChildren<SpeedBoosterMb>();
            var entity = world.NewEntity();
            var packedEntity = world.PackEntity(entity);
            PackedEntity = packedEntity;
            dragAndDrop.PackedEntity = packedEntity;
            speedBooster.PackedEntity = packedEntity;
            world.GetPool<CTaxi>().Add(entity).Invoke(this);
            world.GetPool<CDragObject>().Add(entity).Invoke(dragAndDrop);
            world.GetPool<CSpeedBooster>().Add(entity).Invoke(speedBooster);
            SetLinks();
        }
        
        private void SetLinks()
        {
            Follower = GetComponentInChildren<PathFollower>(true);
            TransparentGfx = GetComponentInChildren<TransparentGFX>(true).transform;
            Follower.speed = Speed;
            Follower.pathCreator = Links.Instance.PathCreator;
            _defaultOffset = TransparentGfx.localPosition;
        }

        public void Configurate(CarsConfig carsConfig)
        {
            Speed = carsConfig.GetSpeed(Level);
            MoneyForCircle = (int)carsConfig.GetProfit(Level);
        }

        public void Drive()
        {
            Follower.distanceTravelled = Random.Range(0, 15);
            Follower.enabled = true;
            Follower.transform.localPosition = _defaultOffset;
            Follower.transform.localRotation = Quaternion.identity;
            Follower.GetComponent<BoxCollider>().enabled = true;
            TransparentGfx.gameObject.SetActive(true);
            SoundManager.Instance.PlayFX(AllSfxSounds.ToPark, transform.position);
        }
    }
}