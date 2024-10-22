using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerTriggerCollliderData
    {
        [field: SerializeField] public BoxCollider GroundCheckCollider { get; private set; }
        public Vector3 GroundCheckColliderExtents { get; private set; }
        
        public void Initialize()
        {
            GroundCheckColliderExtents = GroundCheckCollider.bounds.extents;
        }
    }
}