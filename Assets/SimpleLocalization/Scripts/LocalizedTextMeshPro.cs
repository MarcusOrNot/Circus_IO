//using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization.Scripts
{
    /// <summary>
	/// Localize text component.
	/// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextMeshPro : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            _textMesh.text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}
