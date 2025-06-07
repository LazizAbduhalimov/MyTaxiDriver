using Leopotam.EcsLite;
using UILobby;
using UnityEngine.UI;

namespace UI.Buttons
{
    public struct ERewardVideoClicked {}
  
    public struct CRewardVideoButton : IButton
    {
        public ButtonHandler Handler; 

        public void Invoke(Button button, int entity, EcsWorld world)
        {
            Handler.Invoke<ERewardVideoClicked>(button, entity, world);
        }
    }
}