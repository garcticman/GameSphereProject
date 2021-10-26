using System;
using Base;

namespace Controllers
{
    public class PlayButtonController : IController
    {
        public event Action OnPlayPressed;
        
        public void PlayPressedInvoke()
        {
            OnPlayPressed?.Invoke();
        }
    }
}