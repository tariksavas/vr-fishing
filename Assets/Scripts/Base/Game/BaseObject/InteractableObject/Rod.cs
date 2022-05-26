namespace Base.Game.BaseObject.InteractableObject
{
    using Base.Game.BaseObject.XR;
    using UnityEngine;

    public class Rod : BaseObject
    {
        [SerializeField] private GameObject objectivePopup = null;

        private PortableObject ownPortableObject;

        protected override void Initialize()
        {
            base.Initialize();

            ownPortableObject = GetComponent<PortableObject>();
        }

        protected override void Registration()
        {
            base.Registration();

            ownPortableObject.Receipt += OnReceipt;
        }
        
        protected override void UnRegistration()
        {
            base.Registration();

            ownPortableObject.Receipt -= OnReceipt;
        }

        private void OnReceipt(BaseHand obj)
        {
            objectivePopup.SetActive(false);
        }
    }
}