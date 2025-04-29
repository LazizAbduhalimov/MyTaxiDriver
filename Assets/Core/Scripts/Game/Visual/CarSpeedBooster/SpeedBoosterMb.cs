using Leopotam.EcsLite;
using PrimeTween;
using UnityEngine;

namespace Client.Game
{
    public class SpeedBoosterMb : MonoBehaviour
    {
        public EcsPackedEntity PackedEntity;
        public GameObject Particle;
        public float BoostCoolDown => 3f;
        [HideInInspector] public Transform Trails;
        [HideInInspector] public float BoostPercentage = 5f;
        [HideInInspector] public bool CanBeBoosted = true;
        [HideInInspector] public Sequence? BoostSequence;

        private void Start()
        {
            Trails = GetComponentsInChildren<Transform>()[1];
            Trails.gameObject.SetActive(false);
            Particle.SetActive(true);
        }

        private void OnMouseDown()
        {
            if (CanBeBoosted)
                CommonUtilities.EventsWorld.GetPool<EBoostSpeed>().NewEntity(out _).Invoke(this);
        }
    }
}