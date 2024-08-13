using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFun
{
    public class Player : MonoBehaviour
    {
        protected Vector2 _moveInputValue;

        [SerializeField] protected Movement _movementComponent;
        [SerializeField] protected CameraRigg _cameraRigg;
        [SerializeField] protected InputActionReference _moveInputAction;
        [SerializeField] protected InputActionReference _sprintInputAction;
        [SerializeField] protected InputActionReference _jumpInputAction;

        private void MoveInput(InputAction.CallbackContext context)
        {
            _moveInputValue = context.ReadValue<Vector2>();

            _movementComponent.isMoving = context.performed;
        }

        private void SprintInput(InputAction.CallbackContext context)
        {
            if(context.performed)
                _movementComponent.isSprinting = true;
            else 
                _movementComponent.isSprinting = false;
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            _movementComponent.Jump();
        }

        public virtual void EnableInput()
        {
            _moveInputAction.action.performed += MoveInput;
            _moveInputAction.action.canceled += MoveInput;
            _sprintInputAction.action.performed += SprintInput;
            _sprintInputAction.action.canceled += SprintInput;
            _jumpInputAction.action.performed += JumpInput;
            _cameraRigg.EnableInput();
        }

        public virtual void DisableInput()
        {
            _moveInputAction.action.performed -= MoveInput;
            _moveInputAction.action.canceled -= MoveInput;
            _sprintInputAction.action.performed -= SprintInput;
            _sprintInputAction.action.canceled -= SprintInput;
            _jumpInputAction.action.performed -= JumpInput;
            _cameraRigg.DisableInput();
        }
    }
}