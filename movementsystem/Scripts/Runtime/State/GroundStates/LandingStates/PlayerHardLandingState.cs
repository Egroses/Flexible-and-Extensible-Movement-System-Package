using Runtime.Controller.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.LandingStates
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsHardLandingParameterHash);
            
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.Disable();
            ResetVelocity();
        }
        public override void OnAnimationTransitionEvent()
        {
            playerMovementSMController.ChangeState(playerMovementSMController.IdlingState);
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
        public override void OnAnimationExitEvent()
        {
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.Enable();
        }
        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsHardLandingParameterHash);
            
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.Enable();
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.started += OnMovementStarted;
        }
        protected override void OnMove()
        {
            if (playerMovementSMController.ReusableData.ShouldWalk)
            {
                return;
            }
            playerMovementSMController.ChangeState(playerMovementSMController.RunningState);
        }
        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext obj)
        {
        }
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}