using UnityEngine;

namespace Client
{
    public class BonusGiver : MonoBehaviour
    {
        public void GiveRandomBonus()
        {
            CommonUtilities.EventsWorld.GetPool<EGiveRandomBonus>().NewEntity(out _);
        }
    }
}