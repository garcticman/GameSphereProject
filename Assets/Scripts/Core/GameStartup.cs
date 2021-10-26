using Systems;
using Controllers;
using Factories;
using FiniteStateMachine;
using Services;
using Settings;
using FiniteStateMachine.States;
using FiniteStateMachine.Transitions;
using UnityEngine;
using Views;

namespace Core
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BalloonView balloonViewPrefab;
        [SerializeField] private GameView gameView;
        [SerializeField] private MenuView menuView;
        [SerializeField] private DifficultySettings difficultySettings;

        private readonly SystemManager _systemManager = new SystemManager();
        private readonly StateMachine _stateMachine = new StateMachine();
        private SystemStateBinder _systemStateBinder;
        private GameState _gameState;
        private MenuState _menuState;

        private ScoreService _scoreService;

        private PlayButtonController _playButtonController;
        private BackButtonController _backButtonController;
        private BalloonTouchController _balloonTouchController;
        private BalloonBumpController _balloonBumpController;

        public void Awake()
        {
            SetupControllers();
            SetupStateMachine();
            SetupSystems();
            SetupViews();
        }

        private void SetupStateMachine()
        {
            _gameState = new GameState(gameView);
            _menuState = new MenuState(menuView);

            _stateMachine.AddState(_menuState);
            _stateMachine.AddState(_gameState);

            _stateMachine.AddTransition(new MenuToGameTransition(_menuState, _gameState, _playButtonController));
            _stateMachine.AddTransition(new GameToMenuTransition(_gameState, _menuState, _backButtonController));

            _stateMachine.ForceSet(_menuState);
        }

        private void SetupControllers()
        {
            _playButtonController = new PlayButtonController();
            _backButtonController = new BackButtonController();
            
            _scoreService = new ScoreService(difficultySettings);
            
            _balloonTouchController = new BalloonTouchController(_scoreService);
            _balloonBumpController = new BalloonBumpController(_scoreService);
        }

        private void SetupSystems()
        {
            var balloonFactory = new BalloonFactory(balloonViewPrefab, _balloonBumpController, _balloonTouchController,
                mainCamera, transform, _scoreService, canvas);
            var balloonSystem = new BalloonSystem(balloonFactory, difficultySettings, _scoreService);

            _systemManager.AddInitSystem(balloonSystem);
            _systemManager.AddUpdateSystem(balloonSystem);
            _systemManager.AddDestroySystem(balloonSystem);

            _systemStateBinder = new SystemStateBinder(_systemManager, _stateMachine);
            _systemStateBinder.BindInitSystem(balloonSystem, _gameState);
            _systemStateBinder.BindUpdateSystem(balloonSystem, _gameState);
            _systemStateBinder.BindDestroySystem(balloonSystem, _gameState);
        }

        private void SetupViews()
        {
            menuView.SetControllers(_playButtonController);
            gameView.SetControllers(_backButtonController);
            gameView.SetData(_scoreService);
        }

        private void Start()
        {
            _systemManager.Init();
        }

        private void Update()
        {
            _systemManager.Update();
        }

        private void OnDestroy()
        {
            _systemManager.Destroy();
            _systemStateBinder.Dispose();
        }
    }
}