using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public string Label { get; private set; }

        public ButtonAttribute(string label = "")
        {
            Label = label;
        }
    }
}