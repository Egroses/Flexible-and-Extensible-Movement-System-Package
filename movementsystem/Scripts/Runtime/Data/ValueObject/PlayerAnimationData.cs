using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerAnimationData
    {
        [Header("State Group Parameter Names")] 
        [field:SerializeField] private string groundedParameterName="Grounded";
        [field:SerializeField] private string movingParameterName="Moving";
        [field:SerializeField] private string stoppingParameterName="Stopping";
        [field:SerializeField] private string landingParameterName="Landing";
        [field:SerializeField] private string airborneParameterName="Airborne";
        
        [Header("Grounded Parameter Names")] 
        [field:SerializeField] private string isIdlingParameterName="IsIdling";
        [field:SerializeField] private string isDashingParameterName="IsDashing";
        [field:SerializeField] private string isWalkingParameterName="IsWalking";
        [field:SerializeField] private string isRunningParameterName="IsRunning";
        [field:SerializeField] private string isSprintingParameterName="IsSprinting";
        [field:SerializeField] private string isMediumStoppingParameterName="IsMediumStopping";
        [field:SerializeField] private string isHardStoppingParameterName="IsHardStopping";
        [field:SerializeField] private string isRollingParameterName="IsRolling";
        [field:SerializeField] private string isHardLandingParameterName="IsHardLanding";
        
        [Header("Airborne Parameter Names")] 
        [field:SerializeField] private string isFallingParameterName="IsFalling";
        
        public int GroundedParameterHash { get; private set; }
        public int MovingParameterHash { get; private set; }
        public int StoppingParameterHash { get; private set; }
        public int LandingParameterHash { get; private set; }
        public int AirborneParameterHash { get; private set; }
        
        public int IsIdlingParameterHash { get; private set; }
        public int IsDashingParameterHash { get; private set; }
        public int IsWalkingParameterHash { get; private set; }
        public int IsRunningParameterHash { get; private set; }
        public int IsSprintingParameterHash { get; private set; }
        public int IsMediumStoppingParameterHash { get; private set; }
        public int IsHardStoppingParameterHash { get; private set; }
        public int IsRollingParameterHash { get; private set; }
        public int IsHardLandingParameterHash { get; private set; }
        
        public int IsFallingParameterHash { get; private set; }

        public void Initalize()
        {
            GroundedParameterHash = Animator.StringToHash(groundedParameterName);
            MovingParameterHash = Animator.StringToHash(movingParameterName);
            StoppingParameterHash = Animator.StringToHash(stoppingParameterName);
            LandingParameterHash = Animator.StringToHash(landingParameterName);
            AirborneParameterHash = Animator.StringToHash(airborneParameterName);

            IsIdlingParameterHash = Animator.StringToHash(isIdlingParameterName);
            IsDashingParameterHash = Animator.StringToHash(isDashingParameterName);
            IsWalkingParameterHash = Animator.StringToHash(isWalkingParameterName);
            IsRunningParameterHash = Animator.StringToHash(isRunningParameterName);
            IsSprintingParameterHash = Animator.StringToHash(isSprintingParameterName);
            IsMediumStoppingParameterHash = Animator.StringToHash(isMediumStoppingParameterName);
            IsHardStoppingParameterHash = Animator.StringToHash(isHardStoppingParameterName);
            IsRollingParameterHash = Animator.StringToHash(isRollingParameterName);
            IsHardLandingParameterHash = Animator.StringToHash(isHardLandingParameterName);
            
            IsFallingParameterHash = Animator.StringToHash(isFallingParameterName);
            
        }
    }
}