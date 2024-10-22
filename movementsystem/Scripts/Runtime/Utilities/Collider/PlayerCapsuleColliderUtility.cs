using System;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Utilities.Collider
{
    [Serializable]
    public class PlayerCapsuleColliderUtility : CapsuleColliderUtility
    {
        [field: SerializeField] public PlayerTriggerCollliderData TriggerCollliderData { get; private set; }

        protected override void OnInitialize()
        {
            TriggerCollliderData.Initialize();
        }
    }
}