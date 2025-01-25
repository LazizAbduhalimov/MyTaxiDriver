using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public static class UIUtils
    {
        public static ref T InitButton<T>(Button button, EcsPool<T> pool, int? entity = null) where T : struct, IButton
        {
            var world = pool.GetWorld();
            entity ??= world.NewEntity();
            ref var buttonComponent = ref pool.Add(entity.Value);
            buttonComponent.Invoke(button, entity.Value, world);
            return ref buttonComponent;
        }
        
        public static ref T InitToggle<T>(Toggle toggle, EcsPool<T> pool, int? entity = null) where T : struct, IToggle
        {
            var world = pool.GetWorld();
            entity ??= world.NewEntity();
            ref var toggleComponent = ref pool.Add(entity.Value);
            toggleComponent.Invoke(toggle, entity.Value, world);
            return ref toggleComponent;
        }
    
        public static void ChangeButtonColor(Button button, Color color)
        {
            var colors = button.colors;
            colors.normalColor = color;
            button.colors = colors;
        }

        public static IEcsSystems AddUIEventsDestroyers(this IEcsSystems systems)
        {
            return 
                systems.DelHere<EBuyVehicleClicked>()
                ;
        }
    }
}