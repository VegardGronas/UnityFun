using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFun
{
    public class ThirdPerson : Player
    {
        
        [SerializeField] private Transform _characterTransform;

        [SerializeField] private Vector3 _characterOffset;
        [SerializeField] private float _characterRotateSpeed = 15f;

        private void Awake()
        {
            _cameraRigg.transform.SetParent(null);
            _characterTransform.SetParent(null);
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void Update()
        {
            MovePlayer();
        }

        private void LateUpdate()
        {
            _cameraRigg.SetPostion(transform.position);
            if (_characterTransform != null) MoveCharacter();
        }

        private void MoveCharacter()
        {
            _characterTransform.position = transform.position + _characterOffset;
            _characterTransform.rotation = Quaternion.Slerp(_characterTransform.rotation, transform.rotation, _characterRotateSpeed * Time.deltaTime);
        }

        private void MovePlayer()
        {
            Vector3 forward = _cameraRigg.YawTransform.forward * _moveInputValue.y;
            Vector3 right = _cameraRigg.YawTransform.right * _moveInputValue.x;
            Vector3 direction = forward + right;

            if (direction.magnitude > 0)
            {
                _movementComponent.RotateController(Quaternion.LookRotation(direction));
            }

            _movementComponent.MoveController(direction);
        }
    }
}