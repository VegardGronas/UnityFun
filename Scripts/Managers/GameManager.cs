using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityFun
{
    public enum GameMode { ThirdPerson, FirstPerson }
    public class GameManager : MonoBehaviour
    {
        // Static instance of the GameManager
        private static GameManager _instance;

        // Property to access the singleton instance
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Find existing instance or create a new one
                    _instance = FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }

        private bool _paused = false;
        private GameObject _pauseCanvas;

        [SerializeField] private GamePresets _gamePreset;
        [Tooltip("If null, character will spawn on the gamemanger object position")]
        [SerializeField] private Transform _spawnTarget;
        [SerializeField] private InputActionReference _pauseInputAction;
        [SerializeField] private bool _globalPause = false;

        public GamePresets GamePreset {  get { return _gamePreset; }  }

        private void OnEnable()
        {
            _pauseInputAction.action.performed += PauseInput;
        }

        private void OnDisable()
        {
            _pauseInputAction.action.performed -= PauseInput;
        }

        private void Start()
        {
            _pauseCanvas = Instantiate(_gamePreset.pauseCanvas);
            _pauseCanvas.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;

            SetGameMode();
        }

        private void PauseInput(InputAction.CallbackContext context)
        {
            _paused = !_paused;

            if (_pauseCanvas) _pauseCanvas.SetActive(_paused);

            if (_paused)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (_globalPause)
            {
                if (_paused) Time.timeScale = 0;
                else Time.timeScale = 1;
            }
        }

        private void SetGameMode()
        {
            Vector3 position;
            Quaternion rotation;

            if (_spawnTarget != null)
            {
                position = _spawnTarget.position;
                rotation = _spawnTarget.rotation;
            }
            else
            {
                position = transform.position;
                rotation = transform.rotation;
            }
            Player character;

            switch (_gamePreset.gameMode)
            {
                case GameMode.ThirdPerson:
                    character = _gamePreset.thirdPersonPrefab;

                    if (character == null)
                    {
                        Debug.LogWarning("Could not find player in resource folder. Game wont work");
                        enabled = false;
                        return;
                    }
                    Instantiate(character, position, rotation);
                    break;
                case GameMode.FirstPerson:
                    character = _gamePreset.firstPersonPrefab;

                    if (character == null)
                    {
                        Debug.LogWarning("Could not find player in resource folder. Game wont work");
                        enabled = false;
                        return;
                    }
                    Instantiate(character, position, rotation);
                    break;
            }
        }
    }
}