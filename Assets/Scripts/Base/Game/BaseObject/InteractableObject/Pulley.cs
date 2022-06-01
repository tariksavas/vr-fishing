namespace Base.Game.BaseObject.InteractableObject
{
    using Base.Game.BaseObject.XR;
    using System;
    using UnityEngine;

    public class Pulley : BaseObject
    {
        [SerializeField] private Transform startTransform = null;

        private PortableObject ownPortable;

        public event Action ReceiptPulley;
        public event Action LeftPulley;

        protected override void Initialize()
        {
            base.Initialize();

            ownPortable = GetComponent<PortableObject>();
        }

        protected override void Registration()
        {
            base.Registration();

            ownPortable.Receipt += OnReceipt;
            ownPortable.Left += OnLeft;
        }

        protected override void UnRegistration()
        {
            base.UnRegistration();

            ownPortable.Receipt -= OnReceipt;
            ownPortable.Left -= OnLeft;
        }

        private void OnLeft()
        {
            transform.position = startTransform.position;
            LeftPulley?.Invoke();
        }

        private void OnReceipt(BaseHand obj)
        {
            ReceiptPulley?.Invoke();
        }
    }
}