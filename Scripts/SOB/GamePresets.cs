using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFun
{
    [CreateAssetMenu(fileName = "GamePreset", menuName = "Game/Preset")]
    public class GamePresets : ScriptableObject
    {
        public GameMode gameMode = GameMode.FirstPerson;
        public GameObject pauseCanvas;
        public float gravity = -9f;
        public Player thirdPersonPrefab;
        public Player firstPersonPrefab;
    }
}