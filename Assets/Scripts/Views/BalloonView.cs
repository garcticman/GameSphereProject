using Base;
using Controllers;
using Core;
using Services;
using Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class BalloonView : ViewBase
    {
        private const float EnoughOffsetForBump = 0.01f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D collider;
        
        [SerializeField] private DifficultySettings difficultySettings;

        private Camera _mainCamera;
        private GameState _gameState;

        private Vector3 _targetScale;
        private Vector3 _startScale;

        private float _growingSpeed;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        public void SetData(Camera mainCamera, GameState gameState)
        {
            _mainCamera = mainCamera;
            _gameState = gameState;
        }

        public override void Show()
        {
            SetGrowingSpeed();
            SetTargetScale();
            SetRandomPosition();

            base.Show();

            _gameState.OnDifficultyChange += SetGrowingSpeed;
        }

        public override void Hide()
        {
            base.Hide();

            _gameState.OnDifficultyChange -= SetGrowingSpeed;
        }

        private void SetGrowingSpeed()
        {
            _growingSpeed = difficultySettings.GetDifficulty(_gameState.CurrentDifficulty).growingSpeed;
        }

        private void SetTargetScale()
        {
            transform.localScale = _startScale;
            _targetScale = _startScale * 2;
        }

        private void SetRandomPosition()
        {
            var size = spriteRenderer.size;

            var widthOffset = size.x;
            var heightOffset = size.y;

            var screenWidth = Screen.width - widthOffset;
            var screenHeight = Screen.height - heightOffset;

            var screenPosition = new Vector3(Random.Range(widthOffset, screenWidth),
                Random.Range(heightOffset, screenHeight), _mainCamera.nearClipPlane);

            var worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0;

            transform.position = worldPosition;
        }

        private void OnValidate()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            Refresh();
            HandleTouch();
        }

        private void HandleTouch()
        {
            if (InputService.IsTouchOn(this, _mainCamera))
            {
                Interact<BalloonTouchController>();
                Hide();
            }
        }

        public override void Refresh()
        {
            Grow();
            if (ReadyToBump())
            {
                Interact<BalloonBumpController>();
                Hide();
            }
        }

        private void Grow()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _growingSpeed);
        }

        private bool ReadyToBump()
        {
            return Vector3.Distance(transform.localScale, _targetScale) <= EnoughOffsetForBump;
        }

        private void OnMouseDown()
        {
            Debug.Log("HELLO");
        }
    }
}