using System;
using System.Collections;
using Runtime.State;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Runtime.Controller.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        public PlayerInputAction InputAction { get; private set; }
        public PlayerInputAction.PlayerActions PlayerActions { get; private set; }

        private void Awake()
        {
            InputAction = new PlayerInputAction();
            PlayerActions = InputAction.Player;
        }

        private void OnEnable()
        {
            InputAction.Enable();
        }

        private void OnDisable()
        {
            InputAction.Disable();
        }

        public void DisableActionFor(InputAction action,float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();
            yield return new WaitForSeconds(seconds);
            action.Enable();

        }
    }
}