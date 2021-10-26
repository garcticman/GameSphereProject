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
    // Дабы упростить этот класс, подумываю над DI контейнером, но в этом проекте обойдусь
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        
        [SerializeField] private BalloonView balloonViewPrefab;
        
        [SerializeField] private MenuView menuView;
        [SerializeField] private GameView gameView;
        [SerializeField] private TimerView timerView;
        [SerializeField] private EndGameView endGameView;
        
        [SerializeField] private DifficultySettings difficultySettings;

        private readonly SystemManager _systemManager = new SystemManager();
        private readonly StateMachine _stateMachine = new StateMachine();
        private SystemStateBinder _systemStateBinder;

        private MenuState _menuState;
        private EndlessGameState _endlessGameState;
        private TimeGameState _timeGameState;
        private EndGameState _endGameState;

        private ScoreService _scoreService;
        private DifficultyService _difficultyService;

        private PlayButtonController _playButtonController;
        private BackButtonController _backButtonController;
        private BalloonTouchController _balloonTouchController;
        private BalloonBumpController _balloonBumpController;

        private TimeGameSystem _timeGameSystem;

        public void Awake()
        {
            SetupServices();
            SetupControllers();
            SetupStateMachine();
            SetupSystems();
            SetupTransitions();
            SetupViews();
        }

        private void SetupServices()
        {
            _scoreService = new ScoreService();
            _difficultyService = new DifficultyService(_scoreService, difficultySettings, menuView);
        }

        private void SetupControllers()
        {
            _playButtonController = new PlayButtonController();
            _backButtonController = new BackButtonController(_scoreService);

            _balloonTouchController = new BalloonTouchController(_scoreService);
            _balloonBumpController = new BalloonBumpController(_scoreService);
        }

        private void SetupStateMachine()
        {
            _menuState = new MenuState(menuView);
            _endlessGameState = new EndlessGameState(gameView);
            _timeGameState = new TimeGameState(gameView, timerView);
            _endGameState = new EndGameState(endGameView);

            _stateMachine.AddState(_menuState);
            _stateMachine.AddState(_endlessGameState);
            _stateMachine.AddState(_timeGameState);
            _stateMachine.AddState(_endGameState);

            _stateMachine.ForceSet(_menuState);
        }

        private void SetupSystems()
        {
            var balloonFactory = new BalloonFactory(balloonViewPrefab, _balloonBumpController, _balloonTouchController,
                mainCamera, transform, _difficultyService, canvas);
            var balloonSystem = new EndlessGameSystem(balloonFactory, difficultySettings, _difficultyService);

            _systemManager.AddInitSystem(balloonSystem);
            _systemManager.AddUpdateSystem(balloonSystem);
            _systemManager.AddDestroySystem(balloonSystem);

            _systemStateBinder = new SystemStateBinder(_systemManager, _stateMachine);
            _systemStateBinder.BindInitSystem(balloonSystem, _endlessGameState);
            _systemStateBinder.BindUpdateSystem(balloonSystem, _endlessGameState);
            _systemStateBinder.BindDestroySystem(balloonSystem, _endlessGameState);
            
            _systemStateBinder.BindInitSystem(balloonSystem, _timeGameState);
            _systemStateBinder.BindUpdateSystem(balloonSystem, _timeGameState);
            _systemStateBinder.BindDestroySystem(balloonSystem, _timeGameState);

            _timeGameSystem = new TimeGameSystem(timerView);
            _systemStateBinder.BindInitSystem(_timeGameSystem, _timeGameState);
            _systemStateBinder.BindUpdateSystem(_timeGameSystem, _timeGameState);
        }

        private void SetupTransitions()
        {
            var toEndlessTransition = new MenuToGameTransition(_menuState, _endlessGameState, _playButtonController);
            toEndlessTransition.SetCondition(() => menuView.IsEndlessModeOn);

            _stateMachine.AddTransition(toEndlessTransition);
            _stateMachine.AddTransition(new GameToMenuTransition(_endlessGameState, _menuState, _backButtonController));
            _stateMachine.AddTransition(new GameToEndScreenTransition(_endlessGameState, _endGameState, _scoreService));

            var toTimeTransition = new MenuToGameTransition(_menuState, _timeGameState, _playButtonController);
            toTimeTransition.SetCondition(() => !menuView.IsEndlessModeOn);
            
            _stateMachine.AddTransition(toTimeTransition);
            _stateMachine.AddTransition(new GameToMenuTransition(_timeGameState, _menuState, _backButtonController));
            _stateMachine.AddTransition(new GameToEndScreenTransition(_timeGameState, _endGameState, _scoreService));
            _stateMachine.AddTransition(new GameToEndScreenTransition(_timeGameState, _endGameState, _timeGameSystem));
            
            _stateMachine.AddTransition(new GameToMenuTransition(_endGameState, _menuState, _backButtonController));
        }

        private void SetupViews()
        {
            menuView.SetControllers(_playButtonController);
            
            gameView.SetControllers(_backButtonController);
            gameView.SetData(_scoreService, _difficultyService);
            
            endGameView.SetData(_scoreService);
            endGameView.SetControllers(_backButtonController);
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
            _difficultyService.Dispose();
        }
    }
}