using PrimeTween;
using TMPro;
using UnityEngine;

public class CopyToBuffer : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private Transform _notifier;
    private Tween? _tween;
    
    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = _tmpText.text;
        _tween?.Stop();
        _notifier.gameObject.SetActive(true);
        _tween = Tween.Delay(2, () => _notifier.gameObject.SetActive(false));
    }
}
