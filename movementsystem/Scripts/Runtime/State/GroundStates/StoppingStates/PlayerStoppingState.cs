using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.StoppingStates
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        private PlayerStoppingData _stoppingData;
        public PlayerStoppingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
            _stoppingData = MovementData.StoppingData;
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            SetBaseCameraRecentering();
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.StoppingParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            RotateTowardsTargetRotation();
            
            if (!IsMovingHorizontally())
            {
                return;
            }
            
            DecelerateHorizantally();
        }
        public override void OnAnimationTransitionEvent()
        {
            playerMovementSMController.ChangeState(playerMovementSMController.IdlingState);
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.StoppingParameterHash);
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
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.started -= OnMovementStarted;
        }

        #endregion
        #region Input Methods
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}