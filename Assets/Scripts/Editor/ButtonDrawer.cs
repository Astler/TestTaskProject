using System.Linq;
using System.Reflection;
using Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonMethodDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MethodInfo[] methods = target.GetType()
                .GetMethods(System.Reflection.BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                ButtonAttribute buttonAttribute = method.GetCustomAttributes(typeof(ButtonAttribute), true)
                    .FirstOrDefault() as ButtonAttribute;

                if (buttonAttribute != null)
                {
                    string buttonLabel = string.IsNullOrEmpty(buttonAttribute.Label) ? method.Name : buttonAttribute.Label;
                    if (GUILayout.Button(buttonLabel))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }
}