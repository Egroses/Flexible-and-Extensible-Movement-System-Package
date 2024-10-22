using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.LandingStates
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData _rollData;
        public PlayerRollingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
            _rollData = MovementData.RollData;
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = _rollData.SpeedModifier;
            base.Enter();

            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsRollingParameterHash);
            
            playerMovementSMController.ReusableData.ShouldSprint = false;
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.Disable();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (playerMovementSMController.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.MediumStoppingState);
                return;
            }
            OnMove();
        }

        public override void Exit()
        {
            base.Exit();
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.Enable();
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsRollingParameterHash);
        }

        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext obj)
        {
        }
        #endregion
    }
}