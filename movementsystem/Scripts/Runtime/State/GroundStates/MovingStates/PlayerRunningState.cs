using System;
using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.MovingStates
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerRunData _runData;
        private float _startTime;
        public PlayerRunningState(PlayerMovementStateMachineController playerMovementStateMachineController) : base(playerMovementStateMachineController)
        {
            _runData = MovementData.RunData;
        }
        #region IState Methods
        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = MovementData.RunData.SpeedModifier;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsRunningParameterHash);
            
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;
            _startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (!playerMovementSMController.ReusableData.ShouldWalk)
            {
                return;
            }

            if (Time.time < _startTime + _runData.RunTimeToEnd)
            {
                return;
            }

            StopRunning();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsRunningParameterHash);
        }

        #endregion
        
        #region Main Methods

        private void StopRunning()
        {
            if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)  
            {
                playerMovementSMController.ChangeState(playerMovementSMController.IdlingState);
            }
            
            playerMovementSMController.ChangeState(playerMovementSMController.WalkingState);
        }

        #endregion
        
        #region Input Methods

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            playerMovementSMController.ChangeState(playerMovementSMController.WalkingState);
        }
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementSMController.ChangeState(playerMovementSMController.MediumStoppingState);
            base.OnMovementCanceled(context);
        }

        #endregion
    }
}