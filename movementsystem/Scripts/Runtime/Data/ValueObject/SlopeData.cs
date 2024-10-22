using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class SlopeData
    {
        [field: SerializeField] [field: Range(0f, 1f)] public float StepHeightPercentAge { get; private set; } = 0.25f;
        
        [field: SerializeField] [field: Range(0f, 5f)] public float FloatRayDistance { get; private set; } = 1.2f;
        
        [field: SerializeField] [field: Range(0f, 50f)] public float StepReachForce { get; private set; } = 25f;
    }
}