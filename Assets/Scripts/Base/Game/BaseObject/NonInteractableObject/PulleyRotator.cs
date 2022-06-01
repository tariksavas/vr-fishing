namespace Base.Game.BaseObject.NonInteractableObject
{
    using Base.Game.BaseObject.InteractableObject;
    using System;
    using System.Collections;
    using UnityEngine;

    public class PulleyRotator : BaseObject
    {
        [SerializeField] private Pulley pulley = null;

        private Transform pulleyTransform;
        private Vector3 lastAngle;

        public static event Action<float> turning;

        protected override void Initialize()
        {
            base.Initialize();

            lastAngle = transform.localEulerAngles;
            pulleyTransform = pulley.transform;
        }

        protected override void Registration()
        {
            base.Registration();

            pulley.ReceiptPulley += delegate { StartCoroutine(CheckDirection()); };
            pulley.LeftPulley += delegate { StopAllCoroutines(); };
        }

        protected override void UnRegistration()
        {
            base.UnRegistration();

            pulley.ReceiptPulley -= delegate { StartCoroutine(CheckDirection()); };
            pulley.LeftPulley -= delegate { StopAllCoroutines(); };
        }

        private void Update()
        {
            transform.LookAt(pulleyTransform);
            FixAngles();
        }

        private void FixAngles()
        {
            if (transform.localEulerAngles.y >= 180)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -90, 0);

            else
                transform.localEulerAngles = new Vector3(-(transform.localEulerAngles.x + 180), -90, 0);
        }

        private IEnumerator CheckDirection()
        {
            while (true)
            {
                var currentAngles = transform.localEulerAngles;
                var difference = currentAngles.x - lastAngle.x;

                if (difference == 0)
                    continue;

                if ((currentAngles.y >= 180 && difference < 0) || (currentAngles.y < 180 && difference > 0))
                    turning?.Invoke(Mathf.Abs(difference));

                else
                    turning?.Invoke(-Mathf.Abs(difference));

                lastAngle = transform.localEulerAngles;

                yield return null;
            }
        }
    }
}