using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerRotationData
    {
        [field:SerializeField] public Vector3 TargetRotationReachTime { private set; get; }
    }
}