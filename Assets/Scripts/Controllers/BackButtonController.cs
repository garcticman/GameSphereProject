using System;
using Base;

namespace Controllers
{
    public class BackButtonController : IController
    {
        public event Action OnBackButtonPressed;
        
        public void BackButtonPressedInvoke()
        {
            OnBackButtonPressed?.Invoke();
        }
    }
}