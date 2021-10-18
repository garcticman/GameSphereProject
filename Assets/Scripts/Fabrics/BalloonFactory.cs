using System.Collections.Generic;
using Base;
using Controllers;
using Core;
using UnityEngine;
using Views;

namespace Fabrics
{
    public class BalloonFactory
    {
        private readonly BalloonView _balloonViewPrefab;
        private readonly Camera _mainCamera;
        private readonly Transform _root;
        private readonly GameState _gameState;

        private readonly Stack<BalloonView> _balloonViewPool = new Stack<BalloonView>();

        public BalloonFactory(BalloonView balloonViewPrefab, Camera mainCamera, Transform root, GameState gameState)
        {
            _balloonViewPrefab = balloonViewPrefab;
            _mainCamera = mainCamera;
            _root = root;
            _gameState = gameState;
        }

        public BalloonView SpawnBalloon()
        {
            if (_balloonViewPool.Count != 0)
            {
                return _balloonViewPool.Pop();
            }

            var balloonView = Object.Instantiate(_balloonViewPrefab, _root);
            balloonView.SetData(_mainCamera, _gameState);
            balloonView.OnHide += OnBalloonHideHandler;

            var balloonTouchController = new BalloonTouchController(_gameState);
            var balloonBumpController = new BalloonBumpController(_gameState);
            
            balloonView.SetControllers(balloonTouchController, balloonBumpController);
            
            return balloonView;
        }

        private void OnBalloonHideHandler(ViewBase balloonViewBase)
        {
            _balloonViewPool.Push(balloonViewBase as BalloonView);
        }
    }
}