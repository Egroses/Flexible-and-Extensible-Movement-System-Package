using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.GroundStates.MovingStates
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.MovingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.MovingParameterHash);
        }

        #endregion
    }
}