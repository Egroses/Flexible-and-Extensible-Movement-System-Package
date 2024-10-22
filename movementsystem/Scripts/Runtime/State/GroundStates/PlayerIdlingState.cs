using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.State.GroundStates
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData _idleData;
        public PlayerIdlingState(PlayerMovementStateMachineController playerMovementStateMachineController) : base(playerMovementStateMachineController)
        {
            _idleData = MovementData.IdleData;
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            playerMovementSMController.ReusableData.BackwardsCameraRecenteringData =
                _idleData.BackwardsCameraRecenteringData;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsIdlingParameterHash);
            
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;
            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();
            if ( playerMovementSMController.ReusableData.MovementInput == Vector2.zero ) return;

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

        public override void Exit()
        {
            base.Exit();
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsIdlingParameterHash);
        }

        #endregion
    }
}