using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.MovingStates
{
    public class PlayerSprintState : PlayerMovingState
    {
        private PlayerSprintData _sprintData;
        private bool _keepSprinting;
        private bool _shouldResetSprintState;
        private float _startTime;
        
        public PlayerSprintState(PlayerMovementStateMachineController playerMovementStateMachineController) : base(playerMovementStateMachineController)
        {
            _sprintData = MovementData.SprintData;
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = _sprintData.SpeedModifier;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsSprintingParameterHash);
            
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;
            _shouldResetSprintState = true;
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            
            if (_keepSprinting)
            {
                return;
            }

            if (Time.time < _startTime + _sprintData.SprintToTimeRun)
            {
                return;
            }
            StopSprinting();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsSprintingParameterHash);
            
            if (_shouldResetSprintState)
            {
                _keepSprinting = false;
                playerMovementSMController.ReusableData.ShouldSprint = false;
            }
        }

        #endregion

        #region Main Methods

        private void StopSprinting()
        {
            if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.IdlingState);
                return;
            }
            playerMovementSMController.ChangeState(playerMovementSMController.RunningState);
        }

        #endregion
        
        #region Reusable Methods

        protected override void OnFall()
        {
            _shouldResetSprintState = false;
            base.OnFall();
        }

        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }
        #endregion
        
        #region Input Methods

        private void OnSprintPerformed(InputAction.CallbackContext obj)
        {
            _keepSprinting = true;
            playerMovementSMController.ReusableData.ShouldSprint = true;
        }

        protected override void OnJumpStarted(InputAction.CallbackContext obj)
        {
            _shouldResetSprintState = false;
            base.OnJumpStarted(obj);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementSMController.ChangeState(playerMovementSMController.HardStoppingState);
            base.OnMovementCanceled(context);
        }

        #endregion
    }
}