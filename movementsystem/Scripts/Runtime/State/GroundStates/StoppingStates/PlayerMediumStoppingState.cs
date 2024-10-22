using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.GroundStates.StoppingStates
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsMediumStoppingParameterHash);
            
            playerMovementSMController.ReusableData.DecelerationForce =
                MovementData.StoppingData.MediumDecelerationForce;
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;
            
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.StoppingParameterHash);
        }

        #endregion
    }
}