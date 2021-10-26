using Base;
using Controllers;
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

        [SerializeField] private DifficultySettings difficultySettings;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float bottomOffset;
        [SerializeField] private float topOffset;

        private Camera _mainCamera;
        private Canvas _canvas;
        private ScoreService _scoreService;

        private Vector3 _targetScale;
        private Vector3 _startScale;

        private float _growingSpeed;
        private bool _isInitialized;
        private Vector3 _leftBottomSpawnBorder;
        private Vector3 _rightTopSpawnBorder;

        public void SetData(Camera mainCamera, Canvas canvas, ScoreService scoreService)
        {
            _mainCamera = mainCamera;
            _canvas = canvas;
            _scoreService = scoreService;
        }

        public override void Show()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                Init();
            }

            SetGrowingSpeed();
            SetTargetScale();
            SetRandomPosition();

            base.Show();

            _scoreService.OnDifficultyChange += SetGrowingSpeed;
        }

        private void Init()
        {
            _startScale = transform.localScale;
            InitSpawnBorders();
        }

        private void InitSpawnBorders()
        {
            var scaleFactor = _canvas.scaleFactor;
            var sprite = spriteRenderer.sprite;

            var bottomOffsetWithScaleFactor = bottomOffset * scaleFactor;
            var topOffsetWithScaleFactor = topOffset * scaleFactor;

            var spriteSize = new Vector2(
                sprite.rect.width * _startScale.x,
                sprite.rect.height * _startScale.y);

            var screenSize = new Vector2(Screen.width, Screen.height);
            var nearClipPlane = _mainCamera.nearClipPlane;

            var leftBottomBorderScreen =
                new Vector3(spriteSize.x, spriteSize.y + bottomOffsetWithScaleFactor, nearClipPlane);
            
            var rightTopBorderScreen = new Vector3(screenSize.x - spriteSize.x,
                screenSize.y - spriteSize.y - topOffsetWithScaleFactor, nearClipPlane);

            _leftBottomSpawnBorder =
                _mainCamera.ScreenToWorldPoint(leftBottomBorderScreen);
            _rightTopSpawnBorder
                = _mainCamera.ScreenToWorldPoint(rightTopBorderScreen);
        }

        public override void Hide()
        {
            _scoreService.OnDifficultyChange -= SetGrowingSpeed;
            base.Hide();
        }

        private void SetGrowingSpeed()
        {
            _growingSpeed = difficultySettings.GetDifficulty(_scoreService.CurrentDifficulty).growingSpeed;
        }

        private void SetTargetScale()
        {
            transform.localScale = _startScale;
            _targetScale = _startScale * 2;
        }

        private void SetRandomPosition()
        {
            var position = new Vector3(Random.Range(_leftBottomSpawnBorder.x, _rightTopSpawnBorder.x),
                Random.Range(_leftBottomSpawnBorder.y, _rightTopSpawnBorder.y), _leftBottomSpawnBorder.z);
            transform.position = position;
        }

        private void OnValidate()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
                Interact<BalloonTouchController>(handler => handler.BalloonTouch());
                Hide();
            }
        }

        private void Refresh()
        {
            Grow();
            if (ReadyToBump())
            {
                Interact<BalloonBumpController>(handler => handler.BalloonBump());
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
    }
}