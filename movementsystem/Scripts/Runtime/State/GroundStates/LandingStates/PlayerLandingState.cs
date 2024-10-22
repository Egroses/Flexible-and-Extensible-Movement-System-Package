using Runtime.Controller.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates.LandingStates
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.LandingParameterHash);
            
            DisableCameraRecentering();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.LandingParameterHash);
        }

        #endregion
    }
}