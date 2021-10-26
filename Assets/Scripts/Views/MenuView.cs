using Base;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MenuView : ViewBase
    {
        [SerializeField] private Button playButton;

        public override void Show()
        {
            base.Show();
            playButton.onClick.AddListener(OnPlayButtonClickHandler);
        }

        public override void Hide()
        {
            playButton.onClick.RemoveListener(OnPlayButtonClickHandler);
            base.Hide();
        }

        private void OnPlayButtonClickHandler()
        {
            Interact<PlayButtonController>(handler => handler.PlayPressedInvoke());
        }
    }
}