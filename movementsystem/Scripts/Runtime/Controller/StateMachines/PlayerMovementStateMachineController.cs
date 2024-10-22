using Runtime.Abstract;
using Runtime.Controller.Character;
using Runtime.Data.ValueObject;
using Runtime.State.Airborne;
using Runtime.State.GroundStates;
using Runtime.State.GroundStates.LandingStates;
using Runtime.State.GroundStates.MovingStates;
using Runtime.State.GroundStates.StoppingStates;
using UnityEngine;

namespace Runtime.Controller.StateMachines
{
    public class PlayerMovementStateMachineController : StateMachine
    {
        public PlayerController PlayerController { get; }
        
        public PlayerStateReusableData ReusableData { get; }
 
        
        public PlayerIdlingState IdlingState { get; }
        public PlayerDashingState DashingState  { get; }
        
        
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerSprintState SprintState { get; }

 
        public PlayerLightStoppingState LightStoppingState  { get; }
        public PlayerMediumStoppingState MediumStoppingState  { get; }
        public PlayerHardStoppingState HardStoppingState  { get; }
        
        
        public PlayerJumpingState JumpingState { get; }
        public PlayerFallingState FallingState { get; }
        
        
        public PlayerLightLandingState LightLandingState { get; }
        public PlayerRollingState RollingState { get; }
        public PlayerHardLandingState HardLandingState { get; }
 
        public PlayerMovementStateMachineController(PlayerController playerController)
        {
            PlayerController = playerController;
            
            ReusableData = new PlayerStateReusableData();
            
            IdlingState = new PlayerIdlingState(this);
            DashingState = new PlayerDashingState(this);
            
            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            SprintState = new PlayerSprintState(this);
            
            LightStoppingState = new PlayerLightStoppingState(this);
            MediumStoppingState = new PlayerMediumStoppingState(this);
            HardStoppingState = new PlayerHardStoppingState(this);

            JumpingState = new PlayerJumpingState(this);
            FallingState = new PlayerFallingState(this);

            LightLandingState = new PlayerLightLandingState(this);
            RollingState = new PlayerRollingState(this);
            HardLandingState = new PlayerHardLandingState(this);
        }
    }
}