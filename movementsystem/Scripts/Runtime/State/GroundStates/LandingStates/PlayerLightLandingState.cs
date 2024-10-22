using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.GroundStates.LandingStates
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            base.Enter();
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;
            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();
            if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
            {
                return;
            }
            OnMove();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsMovingHorizontally())
            {
                return;
            }
            ResetVelocity();
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            playerMovementSMController.ChangeState(playerMovementSMController.IdlingState);
        }

        #endregion
    }
}