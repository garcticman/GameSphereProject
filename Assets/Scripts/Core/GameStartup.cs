using System.Collections.Generic;
using Systems;
using Base;
using Factories;
using Settings;
using UnityEngine;
using Views;

namespace Core
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BalloonView balloonViewPrefab;
        [SerializeField] private DifficultySettings difficultySettings;
        [SerializeField] private StatusBarView statusBarView;

        private readonly List<ISystem> _systems = new List<ISystem>();
        
        private GameState _gameState;

        public void Awake()
        {
            _gameState = new GameState(difficultySettings);
            
            SetupViews();
            SetupSystems();
        }

        private void SetupViews()
        {
            statusBarView.SetData(_gameState);
            statusBarView.Show();
        }

        private void SetupSystems()
        {
            var balloonFactory = new BalloonFactory(balloonViewPrefab, mainCamera, transform, _gameState);
            _systems.Add(new BalloonSpawnSystem(balloonFactory, _gameState, difficultySettings));
        }

        private void Start()
        {
            for (var index = 0; index < _systems.Count; index++)
            {
                _systems[index].Init();
            }
        }

        private void Update()
        {
            for (var index = 0; index < _systems.Count; index++)
            {
                _systems[index].Execute();
            }
        }

        private void OnDestroy()
        {
            for (var index = 0; index < _systems.Count; index++)
            {
                _systems[index].Destroy();
            }
        }
    }
}