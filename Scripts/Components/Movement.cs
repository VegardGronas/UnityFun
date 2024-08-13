using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFun
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        private float _verticalVelocity;
        private CharacterController _characterController;
        private float _currentSpeed;

        public float moveSpeed = 2f;
        public float jumpStrength = 5f; // Increased jump strength for better control
        public float gravity = 9.81f; // Customize gravity to match the desired effect

        [Tooltip("This value will be added to the current movespeed (Movespeed + SprintSpeed)")]
        public float sprintSpeed;

        public bool isSprinting = false;
        public bool isMoving = false;
        public float CurrentSpeed { get { return _currentSpeed; } }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity(); // Apply gravity each frame
        }

        public void MoveController(Vector3 direction)
        {
            if (isSprinting)
                _currentSpeed = moveSpeed + sprintSpeed;
            else
                _currentSpeed = moveSpeed;

            // Apply horizontal movement
            Vector3 moveDirection = direction * _currentSpeed;
            moveDirection.y = _verticalVelocity; // Include vertical velocity for jumping and gravity

            _characterController.Move(moveDirection * Time.deltaTime);
        }

        public void RotateController(Quaternion rotation)
        {
            _characterController.transform.rotation = rotation;
        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                // Apply jump strength only if the character is grounded
                _verticalVelocity = jumpStrength;
            }
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
            {
                _verticalVelocity -= gravity * Time.deltaTime; // Apply gravity when not grounded
            }
            else
            {
                // When grounded, reset vertical velocity to a small negative value to stick the player to the ground
                if (_verticalVelocity < 0)
                {
                    _verticalVelocity = -0.1f;
                }
            }
        }
    }
}