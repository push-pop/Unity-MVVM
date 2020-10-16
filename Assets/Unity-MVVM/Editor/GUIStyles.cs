using UnityEngine;

namespace UnityMVVM.Editor
{
    public class GUIStyles
    {

        static GUIStyle _bindingButton;

        public static GUIStyle BindingButton
        {
            get
            {
                if (_bindingButton == null)
                {
                    _bindingButton = new GUIStyle(GUI.skin.button)
                    {
                        richText = true,
                        margin = new RectOffset(0, 0, 0, 0),
                        stretchHeight = false
                    };
                    _bindingButton.active.background = _bindingButton.normal.background;
                }

                return _bindingButton;
            }
        }

        static GUIStyle _bindingLabel;
        public static GUIStyle LabelCenter
        {
            get
            {
                if (_bindingLabel == null)
                    _bindingLabel = new GUIStyle(GUI.skin.label)
                    {
                        richText = true,
                        alignment = TextAnchor.MiddleRight
                    };

                return _bindingLabel;
            }
        }

        public static GUIStyle _labelLeft;

        public static GUIStyle LabelLeft
        {
            get
            {
                if (_labelLeft == null)
                    _labelLeft = new GUIStyle(LabelCenter)
                    {
                        alignment = TextAnchor.MiddleLeft
                    };
                return _labelLeft;
            }
        }
    }
}