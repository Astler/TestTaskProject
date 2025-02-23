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
        [SerializeField] private TextMeshProUGUI sensitivityValueText;
        [SerializeField] private Slider sensitivitySlider;

        public event Action<int> PowerValueChanged;
        public event Action<float> SensitivityValueChanged;

        private void Awake()
        {
            powerSlider.onValueChanged.AddListener(OnPowerValueChanged);
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityValueChanged);

            powerSlider.minValue = ProjectConstants.MinPower;
            powerSlider.maxValue = ProjectConstants.MaxPower;
            sensitivitySlider.minValue = ProjectConstants.MinSensitivity;
            sensitivitySlider.maxValue = ProjectConstants.MaxSensitivity;
        }

        public void SetPower(int value, bool silent = false)
        {
            if (silent)
            {
                powerSlider.SetValueWithoutNotify(value);
                UpdatePowerText(value);
                return;
            }

            powerSlider.value = value;
        }

        private void OnPowerValueChanged(float currentPower)
        {
            int value = Mathf.RoundToInt(currentPower);
            UpdatePowerText(value);
            PowerValueChanged?.Invoke(value);
        }

        private void UpdatePowerText(float value)
        {
            powerValueText.text = value.ToString("N0");
        }


        public void SetSensitivity(float value, bool silent = false)
        {
            if (silent)
            {
                sensitivitySlider.SetValueWithoutNotify(value);
                UpdateSensitivityText(value);
                return;
            }

            sensitivitySlider.value = value;
        }


        private void OnSensitivityValueChanged(float value)
        {
            UpdateSensitivityText(value);
            SensitivityValueChanged?.Invoke(value);
        }

        private void UpdateSensitivityText(float value)
        {
            sensitivityValueText.text = value.ToString("P0");
        }

        private void OnDestroy()
        {
            powerSlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.RemoveAllListeners();
        }
    }
}