namespace Base.Game.BaseObject.NonInteractableObject
{
    using Base.Utility;
    using UnityEngine;

    public class PulleyFollower : BaseObject
    {
        [SerializeField] private Transform pulley = null;

        private float startZPos;

        protected override void Initialize()
        {
            base.Initialize();
            startZPos = transform.localPosition.z;
        }

        private void Update()
        {
            transform.position = pulley.position;
            transform.localPositionZ(startZPos);
        }
    }
}