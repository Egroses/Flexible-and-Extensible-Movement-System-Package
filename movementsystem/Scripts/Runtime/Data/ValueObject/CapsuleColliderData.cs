using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class CapsuleColliderData
    {
        public CapsuleCollider capsuleCollider { get; private set; }
        public Vector3 ColliderCenterInLocalSpace { get; private set; }
        public Vector3 ColliderVerticalExtent { get; private set; }

        public void Initialize(GameObject tempObj)
        {
            if(capsuleCollider!=null) return;

            capsuleCollider = tempObj.GetComponent<CapsuleCollider>();

            UpdateColliderData();
        }

        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = capsuleCollider.center;
            ColliderVerticalExtent = new Vector3(0f, capsuleCollider.bounds.extents.y, 0f);
        }
    }
}