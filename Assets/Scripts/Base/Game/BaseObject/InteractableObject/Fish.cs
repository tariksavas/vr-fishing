namespace Base.Game.BaseObject.InteractableObject
{
    using Base.Game.BaseObject.XR;
    using System;
    using UnityEngine;

    public class Fish : BaseObject
    {
        private PortableObject ownPortableObject;
        private Rigidbody ownRigidbody;

        public event Action<Fish> handled;

        protected override void Initialize()
        {
            base.Initialize();

            ownPortableObject = GetComponent<PortableObject>();
            ownRigidbody = GetComponent<Rigidbody>();
        }

        protected override void Registration()
        {
            base.Registration();

            ownPortableObject.Receipt += OnReceipt;
            ownPortableObject.Left += OnLeft;
        }

        protected override void UnRegistration()
        {
            base.UnRegistration();

            ownPortableObject.Receipt -= OnReceipt;
            ownPortableObject.Left -= OnLeft;
        }

        private void OnReceipt(BaseHand obj)
        {
            handled?.Invoke(this);
        }

        private void OnLeft()
        {
            ownRigidbody.isKinematic = false;
            ownRigidbody.useGravity = true;
        }
    }
}