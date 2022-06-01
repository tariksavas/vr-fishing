namespace Base.Game.BaseObject
{
    using Base.Game.BaseObject.XR;
    using System;
    using UnityEngine;

    public class PortableObject : BaseObject, IInteractable
    {
        public event Action<BaseHand> Receipt;
        public event Action Left;

        private Transform parent;

        protected override void Initialize()
        {
            base.Initialize();
            parent = transform.parent;
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
            transform.SetParent(hand.transform);
            Receipt?.Invoke(hand);
        }

        protected virtual void Leave(BaseHand hand)
        {
            transform.SetParent(parent);
            Left?.Invoke();

            hand.GripButtonDown -= OnGripButtonDown;
            hand.GripButtonUp -= OnGripButtonUp;
        }
    }
}