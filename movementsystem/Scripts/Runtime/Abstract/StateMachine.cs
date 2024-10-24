using Runtime.Controller.StateMachines;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Abstract
{
    public abstract class StateMachine
    {
        protected IState CurrentState;

        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void HandleInput()
        {
            CurrentState?.HandleInput();
        }
        
        public void Update()
        {
            CurrentState?.Update();
        }
        
        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }
        public void OnAnimationEnterEvent()
        {
            CurrentState?.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            CurrentState?.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            CurrentState?.OnAnimationTransitionEvent();
        }

        public void OnTriggerEnter(Collider collider)
        {
            CurrentState?.OnTriggerEnter(collider);
        }
        
        public void OnTriggerExit(Collider collider)
        {
            CurrentState?.OnTriggerExit(collider);
        }
    }
}