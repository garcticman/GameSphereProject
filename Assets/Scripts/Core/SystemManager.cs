using System.Collections.Generic;
using Base;

namespace Core
{
    public class SystemManager
    {
        private readonly List<IInitSystem> _initSystems = new List<IInitSystem>();
        private readonly List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private readonly List<IDestroySystem> _destroySystems = new List<IDestroySystem>();

        public void AddInitSystem(IInitSystem initSystem)
        {
            if (_initSystems.Contains(initSystem)) return;
            
            _initSystems.Add(initSystem);
        }

        public bool ContainsUpdateSystem(IUpdateSystem updateSystem)
        {
            return _updateSystems.Contains(updateSystem);
        }
        
        public void AddUpdateSystem(IUpdateSystem updateSystem)
        {
            _updateSystems.Add(updateSystem);
        }

        public void RemoveUpdateSystem(IUpdateSystem updateSystem)
        {
            _updateSystems.Remove(updateSystem);
        }

        public void AddDestroySystem(IDestroySystem destroySystem)
        {
            if (_destroySystems.Contains(destroySystem)) return;
            
            _destroySystems.Add(destroySystem);
        }

        public void Init()
        {
            for (var index = 0; index < _initSystems.Count; index++)
            {
                _initSystems[index].Init();
            }
        }

        public void Update()
        {
            for (var index = 0; index < _updateSystems.Count; index++)
            {
                _updateSystems[index].Update();
            }
        }

        public void Destroy()
        {
            for (var index = 0; index < _destroySystems.Count; index++)
            {
                _destroySystems[index].Destroy();
            }
        }
    }
}