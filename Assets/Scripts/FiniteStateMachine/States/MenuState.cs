using Views;

namespace FiniteStateMachine.States
{
    public class MenuState : IState
    {
        private readonly MenuView _menuView;

        public bool IsActive { get; private set; }

        public MenuState(MenuView menuView)
        {
            _menuView = menuView;
        }
        
        public void Activate()
        {
            IsActive = true;
            
            _menuView.Show();
        }

        public void Deactivate()
        {
            IsActive = false;
            
            _menuView.Hide();
        }
    }
}