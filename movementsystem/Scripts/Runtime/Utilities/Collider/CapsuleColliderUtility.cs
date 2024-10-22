using System;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Utilities.Collider
{
    [Serializable]
    public class CapsuleColliderUtility
    {
        public CapsuleColliderData CapsuleColliderData { get; private set; }
        [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData SlopeData { get; private set; }

        public void Initialize(GameObject tempObj)
        {
            if (CapsuleColliderData != null)
            {
                return;
            }
            CapsuleColliderData = new CapsuleColliderData();
            
            CapsuleColliderData.Initialize(tempObj);
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            
        }
        
        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentAge));
            ReCalculateCapsuleColliderCenter(DefaultColliderData.CenterY);

            var capsuleColliderHeight = CapsuleColliderData.capsuleCollider.height / 2f;
            
            if (capsuleColliderHeight < CapsuleColliderData.capsuleCollider.radius)
            {
                SetCapsuleColliderRadius(capsuleColliderHeight);
            }
            
            CapsuleColliderData.UpdateColliderData();
        }

        private void ReCalculateCapsuleColliderCenter(float centerY)
        {
            float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.capsuleCollider.height;
            
            Vector3 newColliderCenter =
                new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2f), 0f);

            CapsuleColliderData.capsuleCollider.center = newColliderCenter;
        }

        private void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.capsuleCollider.height = height;
        }

        public void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.capsuleCollider.radius = radius;
        }
    }
}