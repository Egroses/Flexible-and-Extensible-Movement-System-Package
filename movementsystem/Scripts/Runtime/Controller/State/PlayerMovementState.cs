using System.Collections.Generic;
using Runtime.Controller.StateMachines;
using Runtime.Data.ValueObject;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Controller.State
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachineController playerMovementSMController;
        
        protected PlayerGroundedData MovementData;
        protected PlayerAirborneData AirborneData;
        public PlayerMovementState(PlayerMovementStateMachineController _playerMovementSMController)
        {
            playerMovementSMController = _playerMovementSMController;
            MovementData = playerMovementSMController.PlayerController.Data.GroundedData;
            AirborneData = playerMovementSMController.PlayerController.Data.AirborneData;
            
            SetBaseCameraRecentering();
            InitializeData();
        }
        private void InitializeData()
        {
            SetBaseRotationData();
        }
        
        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State : "+GetType().Name);
            AddInputActionCallbacks();
        }
        public virtual void Exit()
        {
            RemoveInputActionCallbacks();
        }
        public virtual void HandleInput()
        {
            ReadMovementInput();
        }
        public virtual void Update()
        {
        }
        public virtual void PhysicsUpdate()
        {
            Move();
        }
        public virtual void OnAnimationEnterEvent()
        {
            
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }
        public virtual void OnTriggerEnter(Collider collider)
        {
            if (playerMovementSMController.PlayerController.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return;
            }
        }
        public virtual void OnTriggerExit(Collider collider)
        {
            if (playerMovementSMController.PlayerController.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExit(collider);
                return;
            }
        }
        #endregion
        
        #region Main Methods

        private void ReadMovementInput()
        {
            playerMovementSMController.ReusableData.MovementInput = playerMovementSMController.PlayerController.Input.PlayerActions.Move
                .ReadValue<Vector2>();
        }
        private void Move()
        {
            if(playerMovementSMController.ReusableData.MovementInput == Vector2.zero || playerMovementSMController.ReusableData.SpeedModifier == 0) return;

            Vector3 movementDirection = GetMovementInputDirection();
            
            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            
            float movementSpeed = GetMovementSpeed();
            
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            
            playerMovementSMController.PlayerController.Rigidbody.AddForce((targetRotationDirection*movementSpeed-currentPlayerHorizontalVelocity),ForceMode.VelocityChange);
        }
        private float Rotate(Vector3 direction)
        {
            var directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();
            
            return directionAngle;
        }
        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
            if (directionAngle < 0) directionAngle += 360;
            
            return directionAngle;
        }
        private float AddCameraRotationToAngle(float directionAngle)
        {
            directionAngle += playerMovementSMController.PlayerController.MainCameraTransform.eulerAngles.y;

            if (directionAngle > 360) directionAngle -= 360;
            return directionAngle;
        }
        private void UpdateTargetRotationData(float targetAngle)
        {
            playerMovementSMController.ReusableData.CurrentTargetRotation.y = targetAngle;
            playerMovementSMController.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }
        #endregion
        
        #region Reusable Methods

        protected void StartAnimation(int animationHash)
        {
            playerMovementSMController.PlayerController.Animator.SetBool(animationHash,true);
        }
        protected void StopAnimation(int animationHash)
        {
            playerMovementSMController.PlayerController.Animator.SetBool(animationHash,false);
        }
        protected virtual void AddInputActionCallbacks()
        {
            playerMovementSMController.PlayerController.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
            playerMovementSMController.PlayerController.Input.PlayerActions.Look.started += OnMouseMovementStarted;
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.performed += OnMovementPerformed;
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.canceled += OnMovementCanceled;
        }
        protected virtual void RemoveInputActionCallbacks()
        {
            playerMovementSMController.PlayerController.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
            playerMovementSMController.PlayerController.Input.PlayerActions.Look.started -= OnMouseMovementStarted;
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.performed -= OnMovementPerformed;
            playerMovementSMController.PlayerController.Input.PlayerActions.Move.canceled -= OnMovementCanceled;
        }
        protected void SetBaseCameraRecentering()
        {
            playerMovementSMController.ReusableData.BackwardsCameraRecenteringData = MovementData.BackwardsCameraRecenteringData;
            playerMovementSMController.ReusableData.SidewaysCameraRecenteringData = MovementData.SidewaysCameraRecenteringData;
        }
        protected void SetBaseRotationData()
        {
            playerMovementSMController.ReusableData.TimeToReachTargetRotation = MovementData.BaseRotationData.TargetRotationReachTime;
            playerMovementSMController.ReusableData.RotationData = MovementData.BaseRotationData;
        }
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(playerMovementSMController.ReusableData.MovementInput.x, 0f, playerMovementSMController.ReusableData.MovementInput.y);
        }
        protected float GetMovementSpeed(bool shouldConsiderSlopes= true)
        {
            float movementSpeed = MovementData.BaseSpeed * playerMovementSMController.ReusableData.SpeedModifier;
            if (shouldConsiderSlopes)
            {
                movementSpeed *= playerMovementSMController.ReusableData.OnSlopeSpeedModifier;
            }
            return  movementSpeed;
        }
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = playerMovementSMController.PlayerController.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, playerMovementSMController.PlayerController.Rigidbody.velocity.y, 0f);
        }
        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle =
                playerMovementSMController.PlayerController.Rigidbody.rotation.eulerAngles.y;
            if(Mathf.Approximately(currentYAngle, playerMovementSMController.ReusableData.CurrentTargetRotation.y)) return;

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, playerMovementSMController.ReusableData.CurrentTargetRotation.y,
                ref playerMovementSMController.ReusableData.DampedTargetRotationCurrentVelocity.y, playerMovementSMController.ReusableData.TimeToReachTargetRotation.y - playerMovementSMController.ReusableData.DampedTargetRotationPassedTime.y);
            
            playerMovementSMController.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
            playerMovementSMController.PlayerController.Rigidbody.MoveRotation(targetRotation);
        }
        protected float UpdateTargetRotation(Vector3 direction,bool shouldConsiderCameraRotation = true)
        {
            float directionAngle =  GetDirectionAngle(direction);
            
            if(shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (!Mathf.Approximately(directionAngle, playerMovementSMController.ReusableData.CurrentTargetRotation.y))
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }
        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        protected void ResetVelocity()
        {
            playerMovementSMController.PlayerController.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            Vector2 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            playerMovementSMController.PlayerController.Rigidbody.velocity = playerHorizontalVelocity;
        }
        protected virtual void DecelerateHorizantally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            
            playerMovementSMController.PlayerController.Rigidbody.AddForce(-playerHorizontalVelocity * playerMovementSMController.ReusableData.DecelerationForce,ForceMode.Acceleration);
        }
        protected virtual void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            
            playerMovementSMController.PlayerController.Rigidbody.AddForce(-playerVerticalVelocity * playerMovementSMController.ReusableData.DecelerationForce,ForceMode.Acceleration);
        }
        protected virtual bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x,playerHorizontalVelocity.z);
            bool isMovingHorizontallyReturn = playerHorizontalMovement.magnitude > minimumMagnitude;
            return isMovingHorizontallyReturn;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }
        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < minimumVelocity;
        }
        protected virtual void OnContactWithGround(Collider collider)
        {
        }
        protected virtual void OnContactWithGroundExit(Collider collider)
        {
        }
        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            if (movementInput == Vector2.up)
            {
                DisableCameraRecentering();
                return;
            }

            float cameraVerticalAngle = playerMovementSMController.PlayerController.MainCameraTransform.eulerAngles.x;
            if (cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            }
            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);
            
            if (movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle,playerMovementSMController.ReusableData.BackwardsCameraRecenteringData);
                return;
            }
            SetCameraRecenteringState(cameraVerticalAngle,playerMovementSMController.ReusableData.SidewaysCameraRecenteringData);
        }
        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movementSpeed = GetMovementSpeed();
            
            if (movementSpeed == 0f)
            {
                movementSpeed = MovementData.BaseSpeed;
            }
            
           playerMovementSMController.PlayerController.CameraUtility.EnableRecentering(waitTime,recenteringTime,MovementData.BaseSpeed,movementSpeed); 
        }
        protected void DisableCameraRecentering()
        {
            playerMovementSMController.PlayerController.CameraUtility.DisableRecentering(); 
        }
        protected void SetCameraRecenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRecenteringData)
        {
            foreach (PlayerCameraRecenteringData recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                {
                    continue;
                }
                EnableCameraRecentering(recenteringData.WaitTime,recenteringData.RecenteringTime);
                return;
            }
            DisableCameraRecentering();
            return;
        }
        #endregion

        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            playerMovementSMController.ReusableData.ShouldWalk = !playerMovementSMController.ReusableData.ShouldWalk;
        }
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();
        }
        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(playerMovementSMController.ReusableData.MovementInput);
        }
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }
        #endregion
    }
}