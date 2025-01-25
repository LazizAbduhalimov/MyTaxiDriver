using Leopotam.EcsLite;
using TMPro;
using UILobby;
using UnityEngine.UI;

namespace UI.Buttons
{
    public struct EBuyVehicleClicked {}
  
    public struct CBuyVehicle : IButton
    {
        public TMP_Text Text;
        public ButtonHandler Handler; 

        public void Invoke(Button button, int entity, EcsWorld world)
        {
            Handler.Invoke<EBuyVehicleClicked>(button, entity, world);
        }
    }
}