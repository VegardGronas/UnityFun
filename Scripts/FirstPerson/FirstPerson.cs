using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFun
{
    public class FirstPerson : Player
    {
        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void Awake()
        {
            _cameraRigg.transform.SetParent(null);
        }

        private void Update()
        {
            MovePlayer();
        }


        private void LateUpdate()
        {
            _cameraRigg.SetPostion(transform.position);
        }

        private void MovePlayer()
        {
            Vector3 forward = _cameraRigg.YawTransform.forward * _moveInputValue.y;
            Vector3 right = _cameraRigg.YawTransform.right * _moveInputValue.x;
            Vector3 direction = forward + right;

            _movementComponent.MoveController(direction);
            _movementComponent.RotateController(_cameraRigg.YawTransform.rotation);
        }
    }
}