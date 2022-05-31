namespace Base.Game.BaseObject.InteractableObject
{
    using Base.Game.BaseObject.XR;
    using System;

    public class Fish : BaseObject
    {
        private PortableObject ownPortableObject;

        public event Action<Fish> handled;
        protected override void Initialize()
        {
            base.Initialize();

            ownPortableObject = GetComponent<PortableObject>();
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

        }
    }
}