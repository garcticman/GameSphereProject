using System;
using UnityEngine;

namespace Base
{
    public abstract class ViewBase : MonoBehaviour
    {
        public event Action<ViewBase> OnShow;
        public event Action<ViewBase> OnHide;
        
        private IController[] _controllers;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            Refresh();
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke(this);
        }
        
        public abstract void Refresh();
        
        public void SetControllers(params IController[] controllers)
        {
            _controllers = controllers;
        }

        protected void Interact<TController, TInteractData>(TInteractData interactData) where TController : IController
        {
            foreach (var controller in _controllers)
            {
                if (!(controller is TController concreteController)) 
                    continue;
            
                concreteController.OnInteract(interactData);
            }
        }
        
        protected void Interact<TController>() where TController : IController
        {
            foreach (var controller in _controllers)
            {
                if (!(controller is TController concreteController)) 
                    continue;
            
                concreteController.OnInteract();
            }
        }
    }
}