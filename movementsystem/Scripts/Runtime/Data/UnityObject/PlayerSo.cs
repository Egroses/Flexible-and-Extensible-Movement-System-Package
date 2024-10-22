using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "PlayerSO", order = 0)]
    public class PlayerSo : ScriptableObject
    {
        [field:SerializeField] public PlayerGroundedData GroundedData { get; private set; }
        [field:SerializeField] public PlayerAirborneData AirborneData { get; private set; }
    }
}