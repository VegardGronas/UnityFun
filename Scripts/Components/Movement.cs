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

        public float moveSpeed = 2f;

        [Tooltip("This value will be added to the current movespeed (Movespeed + SprintSpeed)")]
        public float sprintSpeed;

        public bool isSprinting = false;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity();
        }

        public void MoveController(Vector3 direction)
        {
            if(isSprinting) _characterController.Move((moveSpeed + sprintSpeed) * Time.deltaTime * direction);
            else _characterController.Move(moveSpeed * Time.deltaTime * direction);
        }

        public void RotateController(Quaternion rotation)
        {
            _characterController.transform.rotation = rotation;
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
            {
                _verticalVelocity -= GameManager.Instance.GamePreset.gravity * Time.deltaTime;
            }
            else
            {
                if (_verticalVelocity < 0)
                {
                    _verticalVelocity = -0.1f;
                }
            }

            _characterController.Move(_verticalVelocity * Time.deltaTime * Vector3.down);
        }
    }
}