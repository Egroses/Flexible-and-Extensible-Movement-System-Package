using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.GroundStates.StoppingStates
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsHardStoppingParameterHash);

            playerMovementSMController.ReusableData.DecelerationForce =
                MovementData.StoppingData.HardDecelerationForce;
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;
        }
        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsHardStoppingParameterHash);
        }

        #endregion

        #region Reusable Methods

        protected override void OnMove()
        {
            if (playerMovementSMController.ReusableData.ShouldWalk)
            {
                return;
            }
            
            playerMovementSMController.ChangeState(playerMovementSMController.RunningState);
            
        }

        #endregion
    }
}