using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Game
{
    public class SpeedBoosterMb : MonoBehaviour
    {
        public EcsPackedEntity PackedEntity;
        public GameObject Particle;
        [HideInInspector] public Transform Trails;
        [HideInInspector] public float BoostPercentage = 5f;
        [HideInInspector] public float BoostCoolDown = 5f;
        [HideInInspector] public bool CanBeBoosted = true;

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