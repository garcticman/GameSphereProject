using System;
using Base;
using FiniteStateMachine.Transitions;
using UnityEngine;
using Views;

namespace Systems
{
    public class TimeGameSystem : IInitSystem, IUpdateSystem, IEndGameTransitionInvocator
    {
        public event Action OnEndGame;
        
        private const float StartTime = 30f;

        private readonly TimerView _timerView;

        private float _currentTime;

        public TimeGameSystem(TimerView timerView)
        {
            _timerView = timerView;
        }

        public void Init()
        {
            _currentTime = StartTime;
        }

        public void Update()
        {
            _currentTime -= Time.deltaTime;
            _timerView.Refresh(_currentTime);

            if (_currentTime < 0)
            {
                OnEndGame?.Invoke();
            }
        }
    }
}