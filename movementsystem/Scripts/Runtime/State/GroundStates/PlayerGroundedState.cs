using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.GroundedParameterHash);
            
            UpdateShouldSprintState();
            
            UpdateCameraRecenteringState(playerMovementSMController.ReusableData.MovementInput);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.GroundedParameterHash);
        }

        #endregion
        #region Main Methods
        private void Float()
        {
            var colliderUtilityCapsuleColliderData = playerMovementSMController.PlayerController.ColliderUtility.CapsuleColliderData;
            var colliderUtilitySlopeData = playerMovementSMController.PlayerController.ColliderUtility.SlopeData;
            
            Vector3 capsuleColliderCenterInWorldSpace = colliderUtilityCapsuleColliderData.capsuleCollider.bounds.center;
            
            //float sphereRadius = colliderUtilityCapsuleColliderData.capsuleCollider.radius;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, colliderUtilitySlopeData.FloatRayDistance, playerMovementSMController.PlayerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, downwardsRayFromCapsuleCenter.direction * -1);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);
                
                if(slopeSpeedModifier == 0) return;

                float distanceToFloatingPoint = colliderUtilityCapsuleColliderData.ColliderCenterInLocalSpace.y * playerMovementSMController.PlayerController.transform.localScale.y - hit.distance;
                
                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * colliderUtilitySlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                playerMovementSMController.PlayerController.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier =
                playerMovementSMController.PlayerController.Data.GroundedData.SlopeSpeedAngles.Evaluate(angle);

            if (playerMovementSMController.ReusableData.OnSlopeSpeedModifier != slopeSpeedModifier)
            {
                playerMovementSMController.ReusableData.OnSlopeSpeedModifier = slopeSpeedModifier;
                UpdateCameraRecenteringState(playerMovementSMController.ReusableData.MovementInput);
            }
            return slopeSpeedModifier;
        }
        private bool IsThereGroupUnderneath()
        {

            BoxCollider groundCheckCollider = playerMovementSMController.PlayerController.ColliderUtility
                .TriggerCollliderData.GroundCheckCollider;
            Vector2 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;
            Collider[] overlappedGroundCollider = Physics.OverlapBox(groundColliderCenterInWorldSpace,playerMovementSMController.PlayerController.ColliderUtility.TriggerCollliderData.GroundCheckColliderExtents
                , groundCheckCollider.transform.rotation, playerMovementSMController.PlayerController.LayerData.GroundLayer,QueryTriggerInteraction.Ignore);

            return overlappedGroundCollider.Length > 0;
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            var inputPlayerActions = playerMovementSMController.PlayerController.Input.PlayerActions;
            inputPlayerActions.Dash.started += OnDashStarted;
            inputPlayerActions.Jump.started += OnJumpStarted;
        }
        
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            var inputPlayerActions = playerMovementSMController.PlayerController.Input.PlayerActions;
            inputPlayerActions.Dash.started -= OnDashStarted;
            inputPlayerActions.Jump.started -= OnJumpStarted;
        }
        protected virtual void OnMove()
        {
            if (playerMovementSMController.ReusableData.ShouldSprint)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.SprintState);   
            }
            if (playerMovementSMController.ReusableData.ShouldWalk)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.WalkingState);
                return;
            }
            playerMovementSMController.ChangeState(playerMovementSMController.RunningState);
        }
        private void UpdateShouldSprintState()
        {
            if (!playerMovementSMController.ReusableData.ShouldSprint)
            {
                return;
            }

            if (playerMovementSMController.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            playerMovementSMController.ReusableData.ShouldSprint = false;
        }
        
        protected override void OnContactWithGroundExit(Collider collider)
        {
            base.OnContactWithGroundExit(collider);

            if (IsThereGroupUnderneath())
            {
                return;
            }
            
            Vector3 capsuleColliderCenterInWorldSpace = playerMovementSMController.PlayerController.ColliderUtility.CapsuleColliderData.capsuleCollider.bounds.center;
            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - playerMovementSMController.PlayerController.ColliderUtility.CapsuleColliderData.ColliderVerticalExtent, Vector3.down);
            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, MovementData.GroundToFallRayDistance, playerMovementSMController.PlayerController.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
            
        }
        protected virtual void OnFall()
        {
            playerMovementSMController.ChangeState(playerMovementSMController.FallingState);
        }
        #endregion
        #region Input Methods
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            if(playerMovementSMController.ReusableData.MovementInput != Vector2.zero)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.DashingState);
            }
            return;
        }
        protected virtual void OnJumpStarted(InputAction.CallbackContext obj)
        {
            playerMovementSMController.ChangeState(playerMovementSMController.JumpingState);
        }

        #endregion
    }
}