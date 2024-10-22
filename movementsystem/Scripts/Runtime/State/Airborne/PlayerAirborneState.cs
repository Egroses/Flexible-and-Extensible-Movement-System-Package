using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.Airborne
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.AirborneParameterHash);
            
            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.AirborneParameterHash);
        }

        #endregion
        #region Reusable Methods

        protected override void OnContactWithGround(Collider collider)
        {
            playerMovementSMController.ChangeState(playerMovementSMController.LightLandingState);
        }

        protected virtual void ResetSprintState()
        {
            playerMovementSMController.ReusableData.ShouldSprint=false;
        }
        #endregion
    }
}