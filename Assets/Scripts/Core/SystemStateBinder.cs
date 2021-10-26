using System;
using System.Collections.Generic;
using Base;
using FiniteStateMachine;

namespace Core
{
    public class SystemStateBinder : IDisposable
    {
        private readonly SystemManager _systemManager;
        private readonly StateMachine _stateMachine;

        private readonly Dictionary<IState, List<IInitSystem>> _boundInitSystems =
            new Dictionary<IState, List<IInitSystem>>();

        private readonly Dictionary<IState, List<IUpdateSystem>> _boundUpdateSystems =
            new Dictionary<IState, List<IUpdateSystem>>();

        private readonly Dictionary<IState, List<IDestroySystem>> _boundDestroySystems =
            new Dictionary<IState, List<IDestroySystem>>();

        public SystemStateBinder(SystemManager systemManager, StateMachine stateMachine)
        {
            _systemManager = systemManager;
            _stateMachine = stateMachine;

            _stateMachine.OnStateActivate += OnStateActivate;
            _stateMachine.OnStateDeactivate += OnStateDeactivate;
        }

        private void OnStateActivate(IState state)
        {
            if (_boundInitSystems.TryGetValue(state, out var initSystems))
            {
                for (var index = 0; index < initSystems.Count; index++)
                {
                    initSystems[index].Init();
                }
            }

            if (_boundUpdateSystems.TryGetValue(state, out var updateSystems))
            {
                for (var index = 0; index < updateSystems.Count; index++)
                {
                    var updateSystem = updateSystems[index];
                    if (!_systemManager.ContainsUpdateSystem(updateSystem))
                        _systemManager.AddUpdateSystem(updateSystem);
                }
            }
        }

        private void OnStateDeactivate(IState state)
        {
            if (_boundDestroySystems.TryGetValue(state, out var destroySystems))
            {
                for (var index = 0; index < destroySystems.Count; index++)
                {
                    destroySystems[index].Destroy();
                }
            }

            if (_boundUpdateSystems.TryGetValue(state, out var updateSystems))
            {
                for (var index = 0; index < updateSystems.Count; index++)
                {
                    _systemManager.RemoveUpdateSystem(updateSystems[index]);
                }
            }
        }

        public void BindInitSystem(IInitSystem system, IState state)
        {
            if (!_boundInitSystems.TryGetValue(state, out var initSystems))
            {
                _boundInitSystems[state] = new List<IInitSystem> {system};
                return;
            }

            initSystems.Add(system);
        }

        public void BindUpdateSystem(IUpdateSystem system, IState state)
        {
            if (_boundUpdateSystems.TryGetValue(state, out var updateSystems))
            {
                updateSystems.Add(system);
            }
            else
            {
                _boundUpdateSystems[state] = new List<IUpdateSystem> {system};
            }

            if (!state.IsActive)
            {
                _systemManager.RemoveUpdateSystem(system);
            }
        }

        public void BindDestroySystem(IDestroySystem system, IState state)
        {
            if (!_boundDestroySystems.TryGetValue(state, out var destroySystems))
            {
                _boundDestroySystems[state] = new List<IDestroySystem> {system};
                return;
            }

            destroySystems.Add(system);
        }

        public void Dispose()
        {
            _stateMachine.OnStateActivate -= OnStateActivate;
            _stateMachine.OnStateDeactivate -= OnStateDeactivate;
        }
    }
}