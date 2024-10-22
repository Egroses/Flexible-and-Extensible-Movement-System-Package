using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerSprintData
    {
        [field: SerializeField]
        [field: Range(1, 3)]
        public float SpeedModifier { get; private set; } = 2f;
        
        [field: SerializeField]
        [field: Range(0, 5)]
        public float SprintToTimeRun { get; private set; } = 1f;
    }
}