using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFun
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _mainInput;

        private void OnEnable()
        {
            _mainInput.Enable();
        }
    }
}