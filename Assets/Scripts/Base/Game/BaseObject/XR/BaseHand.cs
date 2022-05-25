namespace Base.Game.BaseObject.XR
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;

    [RequireComponent(typeof(Collider))]
    public class BaseHand : BaseObject
    {
        [SerializeField] private InputDeviceCharacteristics handType = InputDeviceCharacteristics.Right;

        private InputDevice _device;
        private static List<BaseHand> Hands { get; set; } = new List<BaseHand>();

        private bool _pressedTriggerButton = false;
        public bool PressedTriggerButton
        {
            get => _pressedTriggerButton;
            set
            {
                if (value && !_pressedTriggerButton)
                    TriggerButtonDown?.Invoke(this);
                else if (!value && _pressedTriggerButton)
                    TriggerButtonUp?.Invoke(this);

                _pressedTriggerButton = value;
                TriggerButtonPressing?.Invoke(this, value);
            }
        }
        
        private bool _pressedGripButton = false;
        public bool PressedGripButton
        {
            get => _pressedGripButton;
            set
            {
                if (value && !_pressedGripButton)
                    GripButtonDown?.Invoke(this);
                else if (!value && _pressedGripButton)
                    GripButtonUp?.Invoke(this);

                _pressedGripButton = value;
                GripButtonPressing?.Invoke(this, value);
            }
        }

        private Vector2 _axisValue = Vector2.zero;
        public Vector2 AxisValue
        {
            get => _axisValue;
            set
            {
                if (_axisValue != Vector2.zero && value == Vector2.zero)
                    PrimaryAxisUp?.Invoke(this);
                else if (_axisValue == Vector2.zero && value != Vector2.zero)
                    PrimaryAxisDown?.Invoke(this);

                _axisValue = value;
                AxisChanged?.Invoke(this, _axisValue);
            }
        }

        private bool _menuButtonPressed = false;
        public bool MenuButtonPressed
        {
            get => _menuButtonPressed;
            set
            {
                if (value && !_menuButtonPressed)
                    MenuButtonDown?.Invoke();

                _menuButtonPressed = value;
            }
        }

        public event Action<BaseHand> TriggerButtonDown;
        public event Action<BaseHand> TriggerButtonUp;
        public event Action<BaseHand, bool> TriggerButtonPressing;
        public event Action<BaseHand> GripButtonDown;
        public event Action<BaseHand> GripButtonUp;
        public event Action<BaseHand, bool> GripButtonPressing;
        public event Action<BaseHand> PrimaryAxisDown;
        public event Action<BaseHand> PrimaryAxisUp;
        public event Action<BaseHand, Vector2> AxisChanged;
        public event Action MenuButtonDown;

        protected override void Initialize()
        {
            base.Initialize();

            Hands.Add(this);
            AssaignHand();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Hands.Remove(this);
        }

        private void Update()
        {
            InputDetection();
        }

        private void AssaignHand()
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, inputDevices);
            _device = inputDevices.Find(x => x.characteristics.HasFlag(handType));
        }

        private void InputDetection()
        {
            if (_device != null && _device.isValid)
            {
                if (_device.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed))
                    PressedTriggerButton = pressed;
                else
                    PressedTriggerButton = false;

                if (_device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue))
                    AxisValue = axisValue;
                else
                    AxisValue = Vector2.zero;

                if (_device.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
                    PressedGripButton = gripPressed;
                else
                    PressedGripButton = false;

                if (_device.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonPressing))
                    MenuButtonPressed = menuButtonPressing;
                else
                    MenuButtonPressed = false;
            }
            else
            {
                PressedTriggerButton = false;
                AxisValue = Vector2.zero;
                PressedGripButton = false;
                MenuButtonPressed = false;
            }
        }
    }
}