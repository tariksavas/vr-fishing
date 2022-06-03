namespace Base.Game.BaseObject.XR
{
    using Base.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;

    public class BaseHand : BaseObject
    {
        [SerializeField] private InputDeviceCharacteristics handType = InputDeviceCharacteristics.Right;

        private InputDevice _device;
        public InputDevice Device => _device;

        private static List<BaseHand> Hands { get; set; } = new List<BaseHand>();


        private PortableObject _handledObject;
        public PortableObject HandledObject
        {
            get => _handledObject;
            set
            {
                _handledObject = value;

                if (_handledObject)
                {
                    _device.SendHapticImpulse(0u, Constant.HAPTICAMPLITUDE, Constant.HAPTICDURATION);
                    ObjectHandled?.Invoke();
                }
            }
        }

        private IInteractable _triggeredObject;
        private IInteractable TriggeredObject
        {
            get => _triggeredObject;
            set
            {
                if (_triggeredObject != null && _triggeredObject != value)
                    _triggeredObject.OnTriggerExitHand(this);

                if (value != null && _triggeredObject != value)
                    value.OnTriggerEnterHand(this);

                _triggeredObject = value;
            }
        }

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
        public event Action ObjectHandled;
        public static event Action MenuButtonDown;

        protected override void Initialize()
        {
            base.Initialize();

            Hands.Add(this);
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

        private void AssaignHand(Action<InputDevice> device)
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, inputDevices);
            device?.Invoke(inputDevices.Find(x => x.characteristics.HasFlag(handType)));
        }

        private void InputDetection()
        {
            AssaignHand(device =>
            {
                _device = device;

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
            });
        }

        private void OnTriggerStay(Collider other)
        {
            if (HandledObject)
                return;

            if (other.gameObject.layer == Constant.LAYEROFBASEOBJECT)
            {
                if (other.GetComponent<IInteractable>() is IInteractable interactableObject)
                    TriggeredObject = interactableObject;

                else if (other.GetComponentInParent<IInteractable>() is IInteractable interactableObject2)
                    TriggeredObject = interactableObject2;
            }
            else
                TriggeredObject = null;
        }

        private void OnTriggerExit(Collider other)
        {
            if (TriggeredObject == other.gameObject.GetComponent<IInteractable>() || TriggeredObject == other.GetComponentInParent<IInteractable>())
                TriggeredObject = null;
        }
    }
}