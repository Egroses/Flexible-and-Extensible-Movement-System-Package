using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.State.GroundStates
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private readonly PlayerDashData _dashData;
        
        private float _startTime;
        private int _consecutiveDashUsed;
        private bool _shouldKeepRotating;
        
        public PlayerDashingState(PlayerMovementStateMachineController _playerMovementSMController) : base(_playerMovementSMController)
        {
            _dashData = MovementData.DashData;
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementSMController.ReusableData.SpeedModifier = _dashData.SpeedModifier;
            base.Enter();
            
            StartAnimation(playerMovementSMController.PlayerController.AnimationData.IsDashingParameterHash);
            
            playerMovementSMController.ReusableData.RotationData = _dashData.RotationData;
            playerMovementSMController.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;
            Dash();
            
            _shouldKeepRotating = playerMovementSMController.ReusableData.MovementInput != Vector2.zero;
            
            UpdateConsecutiveDashes();

            _startTime = Time.time;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (Time.time - _startTime > 0.1f)
            {
                if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
                {
                    playerMovementSMController.ChangeState(playerMovementSMController.HardStoppingState);
                    return;
                }
            
                playerMovementSMController.ChangeState(playerMovementSMController.SprintState);
            }

            if (!_shouldKeepRotating)
            {
                return;
            }
            
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (playerMovementSMController.ReusableData.MovementInput == Vector2.zero)
            {
                playerMovementSMController.ChangeState(playerMovementSMController.HardStoppingState);
                return;
            }
            
            playerMovementSMController.ChangeState(playerMovementSMController.SprintState);
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(playerMovementSMController.PlayerController.AnimationData.IsDashingParameterHash);
            
            SetBaseRotationData();
        }

        #endregion
        #region Main Methods

        private void Dash()
        {
            Vector3 dashDirection = playerMovementSMController.PlayerController.transform.forward;
            dashDirection.y = 0f;
            UpdateTargetRotation(dashDirection,false);
            if (playerMovementSMController.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementInputDirection());
                dashDirection = GetTargetRotationDirection(playerMovementSMController.ReusableData.CurrentTargetRotation.y);
            }
            playerMovementSMController.PlayerController.Rigidbody.velocity =
                dashDirection * GetMovementSpeed(false);
        }
        
        private void UpdateConsecutiveDashes()
        {
            if (!isConsecutive())
            {
                _consecutiveDashUsed = 0;
            }

            ++_consecutiveDashUsed;

            if (_consecutiveDashUsed == _dashData.ConsecutiveDashesLimitAmount)
            {
                _consecutiveDashUsed = 0;
                
                var playerControllerInput = playerMovementSMController.PlayerController.Input;
                
                playerControllerInput.DisableActionFor(playerControllerInput.PlayerActions.Dash,_dashData.DashLimitReachedCooldown);
            }
        }

        private bool isConsecutive()
        {
            return Time.time < _startTime+_dashData.TimeToConsideredConsecutive;
        }

        #endregion
        #region Reusable Methods

        protected override void AddInputActionCallbacks()
        {
            base.AddInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.performed += OnMovementPerformed;
        }
        protected override void RemoveInputActionCallbacks()
        {
            base.RemoveInputActionCallbacks();
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.performed -= OnMovementPerformed;
        }

        #endregion
        #region Input Methods
        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            _shouldKeepRotating = true;
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }

        #endregion
    }
}