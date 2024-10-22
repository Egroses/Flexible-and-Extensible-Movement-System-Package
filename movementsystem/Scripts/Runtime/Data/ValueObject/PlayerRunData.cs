using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerRunData 
    {
        [field:SerializeField]
        [field: Range(1f, 2f)]
        public float SpeedModifier { get; private set; } = 1f;
        
        [field:SerializeField]
        [field: Range(0f, 5f)]
        public float RunTimeToEnd { get; private set; } = 1f;
    }
}