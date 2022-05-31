namespace Base.Game.BaseObject.NonInteractableObject
{
    using Base.Game.BaseObject.InteractableObject;
    using System;
    using UnityEngine;

    public class RopeEnd : BaseObject
    {
        [SerializeField] private float waterLevel = 1;

        public event Action<bool> underWater;

        private void Update()
        {
            underWater?.Invoke(transform.position.y < waterLevel);
        }

        public void CatchFish(GameObject fishPrefab ,Action<Fish> caught)
        {
            var caughtFish = Instantiate(fishPrefab, transform);
            caughtFish.transform.position = transform.position;
            caught?.Invoke(caughtFish.GetComponent<Fish>());
        }
    }
}