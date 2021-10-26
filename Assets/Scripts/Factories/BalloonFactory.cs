using System.Collections.Generic;
using Base;
using Controllers;
using Core;
using Services;
using FiniteStateMachine.States;
using UnityEngine;
using Views;

namespace Factories
{
    public class BalloonFactory
    {
        private readonly BalloonView _balloonViewPrefab;
        private readonly BalloonBumpController _balloonBumpController;
        private readonly BalloonTouchController _balloonTouchController;
        private readonly Camera _mainCamera;
        private readonly Transform _root;
        private readonly DifficultyService _difficultyService;
        private readonly Canvas _canvas;

        private readonly Stack<BalloonView> _balloonViewPool = new Stack<BalloonView>();

        public BalloonFactory(BalloonView balloonViewPrefab, BalloonBumpController balloonBumpController,
            BalloonTouchController balloonTouchController, Camera mainCamera, Transform root,
            DifficultyService difficultyService, Canvas canvas)
        {
            _balloonViewPrefab = balloonViewPrefab;
            _balloonBumpController = balloonBumpController;
            _balloonTouchController = balloonTouchController;
            _mainCamera = mainCamera;
            _root = root;
            _difficultyService = difficultyService;
            _canvas = canvas;
        }

        public BalloonView SpawnBalloon()
        {
            if (_balloonViewPool.Count != 0)
            {
                return _balloonViewPool.Pop();
            }

            var balloonView = Object.Instantiate(_balloonViewPrefab, _root);

            balloonView.SetData(_mainCamera, _canvas, _difficultyService);
            balloonView.SetControllers(_balloonBumpController, _balloonTouchController);

            balloonView.OnHide += OnBalloonHideHandler;
            
            return balloonView;
        }

        private void OnBalloonHideHandler(ViewBase balloonViewBase)
        {
            _balloonViewPool.Push(balloonViewBase as BalloonView);
        }
    }
}