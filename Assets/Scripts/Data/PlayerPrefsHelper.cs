using UnityEngine;

namespace Data
{
    public static class PlayerPrefsHelper
    {
        private const string SensitivityKey = "sensitivity";

        private static float? _sensitivity;

        public static float Sensitivity
        {
            get => _sensitivity ??= PlayerPrefs.GetFloat(SensitivityKey, ProjectConstants.DefaultSensitivity);
            set
            {
                _sensitivity = value;
                PlayerPrefs.SetFloat(SensitivityKey, value);
                PlayerPrefs.Save();
            }
        }
    }
}