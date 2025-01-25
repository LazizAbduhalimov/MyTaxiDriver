using Leopotam.EcsLite;
using UnityEngine.UI;

namespace UI
{
    public interface IToggle
    {
        public void Invoke(Toggle toggle, int entity, EcsWorld world);
    }
}