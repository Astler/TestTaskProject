using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.GameHud
{
    public class GameHugView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI powerValueText;
        [SerializeField] private Slider powerSlider;

        public event Action<int> PowerValueChanged;

        private void Awake()
        {
            powerSlider.onValueChanged.AddListener(OnPowerValueChanged);
        }

        public void SetPower(int value, bool silent = false)
        {
            if (silent)
            {
                powerSlider.SetValueWithoutNotify(value);
                UpdateValueText(value);
                return;
            }

            powerSlider.value = value;
        }

        private void OnPowerValueChanged(float currentPower)
        {
            int value = Mathf.RoundToInt(currentPower);
            UpdateValueText(value);
            PowerValueChanged?.Invoke(value);
        }

        private void UpdateValueText(float value)
        {
            powerValueText.text = value.ToString("N0");
        }

        private void OnDestroy()
        {
            powerSlider.onValueChanged.RemoveAllListeners();
        }
    }
}