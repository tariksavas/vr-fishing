namespace Base.Game.BaseObject
{
    using Base.Game.BaseObject.XR;
    using System;
    using UnityEngine;

    public class PortableObject : BaseObject, IInteractable
    {
        [SerializeField] private bool returnStartParent = true;

        private Rigidbody ownRigidbody;
        private bool startGravityAction = false;
        private bool startKinematicAction = false;

        public event Action<BaseHand> Receipt;
        public event Action Left;

        private Transform parent;

        protected override void Initialize()
        {
            base.Initialize();

            parent = transform.parent;
            ownRigidbody = GetComponent<Rigidbody>();
        }

        public void OnTriggerEnterHand(BaseHand hand)
        {
            hand.GripButtonDown += OnGripButtonDown;
            hand.GripButtonUp += OnGripButtonUp;
        }

        public void OnTriggerExitHand(BaseHand hand)
        {
            if (hand.HandledObject == this)
                return;

            hand.GripButtonDown -= OnGripButtonDown;
            hand.GripButtonUp -= OnGripButtonUp;
        }

        private void OnGripButtonDown(BaseHand hand)
        {
            Receive(hand);
        }

        private void OnGripButtonUp(BaseHand hand)
        {
            Leave(hand);
        }

        protected virtual void Receive(BaseHand hand)
        {
            hand.HandledObject = this;

            startGravityAction = ownRigidbody.useGravity;
            startKinematicAction = ownRigidbody.isKinematic;
            ownRigidbody.useGravity = false;
            ownRigidbody.isKinematic = true;

            transform.SetParent(hand.transform);
            Receipt?.Invoke(hand);
        }

        protected virtual void Leave(BaseHand hand)
        {
            hand.HandledObject = null;

            ownRigidbody.useGravity = startGravityAction;
            ownRigidbody.isKinematic = startKinematicAction;

            transform.SetParent(returnStartParent ? parent : null);
            Left?.Invoke();

            hand.GripButtonDown -= OnGripButtonDown;
            hand.GripButtonUp -= OnGripButtonUp;
        }
    }
}