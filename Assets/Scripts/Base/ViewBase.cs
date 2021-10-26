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
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke(this);
        }

        public void SetControllers(params IController[] controllers)
        {
            _controllers = controllers;
        }

        protected void Interact<TController>(Action<TController> action) where TController : IController
        {
            foreach (var controller in _controllers)
            {
                if (!(controller is TController concreteController))
                    continue;

                action.Invoke(concreteController);
            }
        }
    }
}