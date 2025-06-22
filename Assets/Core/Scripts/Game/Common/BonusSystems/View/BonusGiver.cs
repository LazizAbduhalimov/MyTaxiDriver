using UnityEngine;

namespace Game
{
    public class BonusGiver : MonoBehaviour
    {
        public void GiveRandomBonus()
        {
            CommonUtilities.EventsWorld.GetPool<EGiveRandomBonus>().NewEntity(out _);
        }
    }
}