using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Game.Common
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private float _updateInterval = 0.5f;
        [SerializeField] private int _smoothingFrames = 5;
        private TMP_Text _text;
        private Queue<float> _frameTimes = new();

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            StartCoroutine(UpdateFPS());
        }

        private IEnumerator UpdateFPS()
        {
            var wait = new WaitForSecondsRealtime(_updateInterval);
            while (true)
            {
                _frameTimes.Enqueue(Time.unscaledDeltaTime);
                if (_frameTimes.Count > _smoothingFrames)
                    _frameTimes.Dequeue();

                var avgDeltaTime = 0f;
                foreach (var t in _frameTimes)
                    avgDeltaTime += t;
                avgDeltaTime /= _frameTimes.Count;

                var fps = (int)(1f / Mathf.Max(avgDeltaTime, 0.0001f));
                _text.text = $"{fps}";

                yield return wait;
            }
        }
    }
}