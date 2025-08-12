using PrimeTween;
using UnityEngine;

namespace Core
{
    public class ScaleChangerOverTime : MonoBehaviour
    {
        [SerializeField] private float _maxScale = 1.2f;
        [SerializeField] private float _duration = 0.25f;
        private Sequence _tween; 
        
        private void OnEnable()
        {
            _tween = Sequence.Create(cycles: int.MaxValue)
                .Chain(Sequence.Create(cycles: 2)
                    .Chain(Tween.Scale(transform, _maxScale, _duration))
                    .Chain(Tween.Scale(transform, 1f, _duration)))
                .Chain(Tween.Delay(1))
                ;
        }

        private void OnDisable()
        {
            _tween.Complete();
        }
    }
}