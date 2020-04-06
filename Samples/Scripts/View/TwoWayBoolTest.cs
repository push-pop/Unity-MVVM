using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityMVVM.View;

namespace UnityMVVM.Examples
{
    public class UnityBoolEvent : UnityEvent<bool> { }

    public class TwoWayBoolTest : ViewBase
    {
        [SerializeField]
        Image _indicator;

        [SerializeField]
        Button _btn;


        public bool BoolProp
        {
            get => _boolProp;
            set
            {
                if (_boolProp != value)
                {
                    _boolProp = value;
                    _indicator.color = _boolProp ? Color.blue : Color.red;

                    OnBoolChanged?.Invoke(value);
                }
            }
        }

        bool _boolProp;

        public UnityBoolEvent OnBoolChanged { get; set; } = new UnityBoolEvent();

        private void Awake()
        {

            _btn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
            {
                BoolProp = !BoolProp;
            }));
        }
    }
}