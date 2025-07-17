using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

namespace Core
{
    public class LocalizeTMPText : MonoBehaviour
    {
        public string LocalizationKey;
        private TMP_Text _text;

        public void Start()
        {
            _text = GetComponent<TMP_Text>();
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            _text.text = LocalizationManager.Localize(LocalizationKey).Replace("\\n", "\n");
        }
    }
}