using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFun
{
    public class CameraRigg : MonoBehaviour
    {
        private Camera _camera;
        private float _currentYawRotation;
        private float _currentPitchRotation;
        private Vector2 _lookInputValue;

        [Tooltip("Offset applied whenever following another gameobject")]
        [SerializeField]private Vector3 _offset;
        [SerializeField] private InputActionReference _lookInputAction;
        [SerializeField] private Transform _yawController;
        [SerializeField] private Transform _pitchController;
        [SerializeField] private Transform _cameraSlot;
        [SerializeField] private LayerMask _collisionLayers;

        public bool invertYaw;
        public bool invertPitch;
        public bool performCollision = true;
        public bool usePitchRotation = false;
        public bool useYawRotation = false;
        public float horizontalSensitivity = 10f;
        public float verticalSensitivity = 10f;

        public Transform YawTransform { get { return _yawController; } }
        public Transform PitchTransform { get { return _pitchController; } }

        private void Start()
        {
            _camera = Camera.main;
            if (!_camera)
            {
                Debug.LogWarning("No main camera in scene, script disabled");
                enabled = false;
            }

            SlotCamera();
        }

        private void Update()
        {
            UpdateRigg();
        }

        private void FixedUpdate()
        {
            if (performCollision) Collision();
        }

        public void SetPostion(Vector3 position)
        {
            Vector3 targetPos = position + _offset;
            transform.position = targetPos;
        }

        private void Collision()
        {
            if (_yawController == null || _pitchController == null || _camera == null || _cameraSlot == null)
            {
                Debug.LogError("One or more required components are missing.");
                return;
            }

            Vector3 rayStart = _yawController.position;
            Vector3 direction = (_cameraSlot.position - _yawController.position).normalized;
            float distance = Vector3.Distance(_yawController.position, _cameraSlot.position);

            RaycastHit hit;

            Debug.DrawRay(rayStart, direction * distance, Color.yellow);

            if (Physics.Raycast(rayStart, direction, out hit, distance, _collisionLayers))
            {
                _camera.transform.position = hit.point;
            }
            else
            {
                _camera.transform.localPosition = Vector3.zero;
            }
        }

        private void UpdateRigg()
        {
            if (_lookInputValue.magnitude > 0)
            {
                if(!invertPitch) _currentPitchRotation += _lookInputValue.y * verticalSensitivity * Time.deltaTime;
                else _currentPitchRotation -= _lookInputValue.y * verticalSensitivity * Time.deltaTime;
                
                if(!invertYaw) _currentYawRotation += _lookInputValue.x * horizontalSensitivity * Time.deltaTime;
                else _currentYawRotation -= _lookInputValue.x * horizontalSensitivity * Time.deltaTime;

                _currentPitchRotation = Mathf.Clamp(_currentPitchRotation, -90, 90);

                _yawController.rotation = Quaternion.Euler(Vector3.up * _currentYawRotation);
                _pitchController.localRotation = Quaternion.Euler(Vector3.right * _currentPitchRotation);
            }
        }

        public void EnableInput()
        {
            _lookInputAction.action.performed += LookInput;
            _lookInputAction.action.canceled += LookInput;
        }

        public void DisableInput()
        {
            _lookInputAction.action.performed -= LookInput;
            _lookInputAction.action.canceled -= LookInput;

            _lookInputValue = Vector2.zero;
        }

        private void LookInput(InputAction.CallbackContext context)
        {
            _lookInputValue = context.ReadValue<Vector2>();
        }

        private void SlotCamera()
        {
            if (_camera != null)
            {
                _camera.transform.SetParent(_cameraSlot);
                _camera.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            }
        }

        private void OnDrawGizmos()
        {
            // Ensure that the Gizmos are only drawn in the Scene view
            if (!Application.isPlaying)
            {
                // Draw a wireframe sphere at the position of _yawController
                if (_yawController != null)
                {
                    Gizmos.color = Color.red; // Set color for yaw controller
                    Gizmos.DrawWireSphere(_yawController.position, 0.5f); // Adjust radius as needed
                }

                // Draw a wireframe sphere at the position of _pitchController
                if (_cameraSlot != null)
                {
                    Gizmos.color = Color.blue; // Set color for pitch controller
                    Gizmos.DrawWireSphere(_cameraSlot.position, 0.5f); // Adjust radius as needed
                }

                // Draw a line connecting the yaw and pitch controllers
                if (_yawController != null && _cameraSlot != null)
                {
                    Gizmos.color = Color.green; // Set color for connection line
                    Gizmos.DrawLine(_yawController.position, _cameraSlot.position);
                }
            }
        }
    }
}