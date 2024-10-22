using Runtime.Controller.StateMachines;
using UnityEngine;

namespace Runtime.State.GroundStates.StoppingStates
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            playerMovementSMController.ReusableData.DecelerationForce =
                MovementData.StoppingData.LightDecelerationForce;
            
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }
    }
}