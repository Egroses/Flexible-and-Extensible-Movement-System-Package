using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.State.Airborne
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private PlayerFallData _fallData;
        private Vector3 _playerPositionOnEnter;
        public PlayerFallingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
            _fallData = AirborneData.FallData;
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsFallingParameterHash);
            
            _playerPositionOnEnter = playerMovementSMController.PlayerController.transform.position;
            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            LimitVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsFallingParameterHash);
        }

        #endregion
        #region Main Methods

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            if (playerVerticalVelocity.y >= -_fallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -_fallData.FallSpeedLimit-playerVerticalVelocity.y, 0f);
            
            playerMovementSMController.PlayerController.Rigidbody.AddForce(limitedVelocity,ForceMode.VelocityChange);
        }
        #endregion

        #region Reusable Methods

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = _playerPositionOnEnter.y -
                                           playerMovementSMController.PlayerController.transform.position.y;
            if (fallDistance < _fallData.MinimumDistanceToBeConsideredHardFall)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.LightLandingState);
                return;
            }

            if (playerMovementSMController.ReusableData.ShouldWalk && !playerMovementSMController.ReusableData.ShouldSprint || playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.HardLandingState);
                return;
            }
            playerMovementSMController.ChangeState(playerMovementSMController.RollingState);
            
        }

        #endregion
    }
}