using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.MovingStates
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData _walkData;
        public PlayerWalkingState(PlayerMovementStateMachineController playerMovementStateMachineController) : base(playerMovementStateMachineController)
        {
            _walkData = MovementData.WalkData;
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = _walkData.SpeedModifier;
            playerMovementSMController.ReusableData.BackwardsCameraRecenteringData =
                _walkData.BackwardsCameraRecenteringData;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsWalkingParameterHash);
            
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsWalkingParameterHash);
            
            SetBaseCameraRecentering();
        }

        #endregion
        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            
            playerMovementSMController.ChangeState(playerMovementSMController.RunningState);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementSMController.ChangeState(playerMovementSMController.LightStoppingState);
            base.OnMovementCanceled(context);
        }

        #endregion
    }
}