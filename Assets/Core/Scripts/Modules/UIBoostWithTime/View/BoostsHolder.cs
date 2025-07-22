using System.Collections.Generic;
using PoolSystem.Alternative;
using UnityEngine;

namespace UI
{
    public class BoostsHolder : MonoBehaviour
    {
        public Dictionary<string, BoostMb> CurrentBoosts = new();
        public PoolContainer PoolContainer;

        public BoostMb SetBoost(Sprite icon, float duration, string id)
        {
            var boostMb = PoolContainer.GetFromPool<BoostMb>();
            boostMb.Setup(icon, duration);
            // CurrentBoosts.Add(id,  boostMb);
            return boostMb;
        }
    }
}