using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.Airborne
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private PlayerJumpData _jumpData;
        private bool _shouldKeepRorating;
        private bool _canStartFalling;
        public PlayerJumpingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
            _jumpData = AirborneData.JumpData;
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementSMController.ReusableData.SpeedModifier = 0f;
            playerMovementSMController.ReusableData.DecelerationForce = _jumpData.DecelerationForce;
            playerMovementSMController.ReusableData.RotationData = _jumpData.RotationData;
            _shouldKeepRorating = playerMovementSMController.ReusableData.MovementInput != Vector2.zero;
            Jump();
        }

        public override void Update()
        {
            base.Update();
            if (!_canStartFalling && IsMovingUp())
            {
                _canStartFalling = true;
            }

            if (!_canStartFalling || GetPlayerVerticalVelocity().y > 0f)
            {
                return;
            }
            
            playerMovementSMController.ChangeState(playerMovementSMController.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (_shouldKeepRorating)
            {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }

        public override void Exit()
        {
            base.Exit();
            SetBaseRotationData();
            _canStartFalling = false;
        }
        #endregion
        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = playerMovementSMController.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = playerMovementSMController.PlayerController.transform.forward;

            if (_shouldKeepRorating)
            {
                UpdateTargetRotation(GetMovementInputDirection());
                jumpDirection =
                    GetTargetRotationDirection(playerMovementSMController.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;
            
            Vector3 capsuleColliderCenterInWorldSpace = playerMovementSMController.PlayerController.ColliderUtility.CapsuleColliderData.capsuleCollider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out var hit,_jumpData.JumpToGroundRayDistance, playerMovementSMController.PlayerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = _jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);
                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = _jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);
                    jumpForce.y *= forceModifier;
                }
            }
            
            if (GetPlayerHorizontalVelocity().magnitude < 0.1f)//If Player Running into wall
            {
                jumpForce.x = 0f;
                jumpForce.z = 0f;
            }
            ResetVelocity();
            playerMovementSMController.PlayerController.Rigidbody.AddForce(jumpForce,ForceMode.VelocityChange);
        }
        #endregion
        #region Reusable

        protected override void ResetSprintState()
        {
            
        }

        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}