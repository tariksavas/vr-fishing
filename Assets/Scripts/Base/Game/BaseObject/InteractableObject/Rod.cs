namespace Base.Game.BaseObject.InteractableObject
{
    using Base.Game.BaseObject.NonInteractableObject;
    using Base.Game.BaseObject.XR;
    using Base.Utility;
    using Obi;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Rod : BaseObject
    {
        [Header("Objective")]
        [SerializeField] private GameObject objectivePopup = null;

        [Header("Fishing"), Space]
        [SerializeField] private string catchAnimName = "Catch";
        [SerializeField] private string releaseAnimName = "Release";
        [SerializeField] private float minFishingTime = 10;
        [SerializeField] private float maxFishingTime = 20;
        [SerializeField] private RopeEnd robeEnd = null;
        [SerializeField] private List<GameObject> fishs = new List<GameObject>();

        private PortableObject ownPortableObject;
        private Coroutine fishingCoroutine;
        private Animator ownAnimator;
        private ObiRopeCursor cursor;
        private ObiRope rope;

        public static event Action catchFish;

        protected override void Initialize()
        {
            base.Initialize();

            ownPortableObject = GetComponent<PortableObject>();
            ownAnimator = GetComponent<Animator>();
            cursor = GetComponentInChildren<ObiRopeCursor>();
            rope = cursor.GetComponent<ObiRope>();
        }

        protected override void Registration()
        {
            base.Registration();

            ownPortableObject.Receipt += OnReceipt;
            ownPortableObject.Left += OnLeft;
            PulleyRotator.turning += OnTurning;
        }

        protected override void UnRegistration()
        {
            base.Registration();

            ownPortableObject.Receipt -= OnReceipt;
            ownPortableObject.Left -= OnLeft;
            PulleyRotator.turning -= OnTurning;
        }

        private void OnReceipt(BaseHand obj)
        {
            if (objectivePopup)
                objectivePopup?.SetActive(false);

            robeEnd.underWater += OnUnderWater;
        }
        private void OnLeft()
        {
            robeEnd.underWater -= OnUnderWater;

            OnUnderWater(false);
        }

        private void OnUnderWater(bool underWater)
        {
            if (!underWater && fishingCoroutine != null)
                StopCoroutine(fishingCoroutine);

            else if (fishingCoroutine == null)
                fishingCoroutine = StartCoroutine(Fishing());
        }

        private IEnumerator Fishing()
        {
            yield return new WaitForSeconds(Random.Range(minFishingTime, maxFishingTime));

            robeEnd.CatchFish(fishs.Random(), caughtFish =>
            {
                if (caughtFish)
                {
                    ownAnimator.SetTrigger(catchAnimName);
                    caughtFish.handled += OnHandled;
                }
            });
        }

        private void OnHandled(Fish caughtFish)
        {
            ownAnimator.SetTrigger(releaseAnimName);
            caughtFish.handled -= OnHandled;
        }

        private void OnTurning(float difference)
        {
            if ((difference < 0 && rope.restLength <= 1.8f) || (difference > 0 && rope.restLength >= 6))
                return;

            cursor.ChangeLength(rope.restLength + difference * Time.deltaTime * 0.02f);
        }
    }
}