using Leopotam.EcsLite;
using UILobby;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public struct ETwoBonusesVideoClicked {}
  
    public struct CTwoBonusesVideo : IButton
    {
        public ButtonHandler Handler;
        public Transform Parent;
        
        public void Invoke(Button button, int entity, EcsWorld world)
        {
            Handler.Invoke<ETwoBonusesVideoClicked>(button, entity, world);
        }
    }
}