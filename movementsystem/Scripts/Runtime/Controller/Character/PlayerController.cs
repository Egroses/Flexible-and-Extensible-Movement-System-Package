using System;
using Runtime.Controller.Input;
using Runtime.Controller.Layer;
using Runtime.Controller.State;
using Runtime.Controller.StateMachines;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Utilities.Cameras;
using Runtime.Utilities.Collider;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Controller.Character
{
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [field: Header("References")] [field: SerializeField] public PlayerSo Data { get; private set; }
        
        [field: Header("Collisions")] [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
       
        [field: Header("Layer Mask")] [field: SerializeField] public LayerLayerData LayerData { get; private set; }
        [field: Header("Cameras")] [field: SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }
        
        [field:Header("Animations")] [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInputController Input { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        private PlayerMovementStateMachineController _playerMovementStateMachineController;
        
        private void Awake()
        {
            this.Rigidbody = GetComponent<Rigidbody>();
            this.Animator = GetComponentInChildren<Animator>();
            
            _playerMovementStateMachineController = new PlayerMovementStateMachineController(this);
            Input = GetComponent<PlayerInputController>();
            
            ColliderUtility.Initialize(this.gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
            CameraUtility.Initialize();
            AnimationData.Initalize();
            
            MainCameraTransform = Camera.main?.transform;

            if (MainCameraTransform == null)
            {
                Debug.LogWarning("Main Camera couldn't find!");
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            ColliderUtility.Initialize(this.gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
        }
#endif
        private void Start()
        {
            _playerMovementStateMachineController.ChangeState(_playerMovementStateMachineController.IdlingState);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnTriggerEnter(Collider collider)
        {
            _playerMovementStateMachineController.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            _playerMovementStateMachineController.OnTriggerExit(collider);
        }

        private void Update()
        {
            _playerMovementStateMachineController.HandleInput();
            _playerMovementStateMachineController.Update();
        }

        private void FixedUpdate()
        {
            _playerMovementStateMachineController.PhysicsUpdate();
        }

        public void OnMovementStateAnimationEnterEvent()
        {
            _playerMovementStateMachineController.OnAnimationEnterEvent();
        }
        public void OnMovementStateAnimationExitEvent()
        {
            _playerMovementStateMachineController.OnAnimationExitEvent();
        }
        public void OnMovementStateAnimationTransitionEvent()
        {
            _playerMovementStateMachineController.OnAnimationTransitionEvent();
        }
    }
}