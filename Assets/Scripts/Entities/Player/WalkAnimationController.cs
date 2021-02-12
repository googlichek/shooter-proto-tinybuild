using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class WalkAnimationController : TickComponent
    {
        [SerializeField] [Range(0, 100)] private float _jumpPower = 0;
        [SerializeField] [Range(0, 100)] private float _sideOffset = 0;
        [SerializeField] [Range(0, 2)] private float _jumpDuration = 0;
        [SerializeField] [Range(0, 2)] private float _walkResetDuration = 0;

        [Space]

        [SerializeField] private Ease _walkEase = Ease.Linear;

        private Sequence _walkSequence;

        private Vector3 _initialLocalPosition;
        private Vector3 _rightLocalPosition;
        private Vector3 _leftLocalPosition;

        private Vector3 _modelRootLocalPosition;
        private Vector3 _resetSmoothing;

        private Vector2 _inputVector;

        public override void Init()
        {
            _initialLocalPosition = transform.localPosition;
            _rightLocalPosition = new Vector3(_initialLocalPosition.x + _sideOffset, _initialLocalPosition.y, _initialLocalPosition.z);
            _leftLocalPosition = new Vector3(_initialLocalPosition.x - _sideOffset, _initialLocalPosition.y, _initialLocalPosition.z);
        }

        public override void Enable()
        {
            _modelRootLocalPosition = _initialLocalPosition;
            transform.localPosition = _modelRootLocalPosition;
        }

        public override void Tick()
        {
            CalculateInputVector();
            HandleWalking();
        }

        private void CalculateInputVector()
        {
            _inputVector.x = GameManager.Instance.InputWrapper.LeftStickHorizontal;
            _inputVector.y = GameManager.Instance.InputWrapper.LeftStickVertical;

            if (_inputVector.magnitude > 1)
                _inputVector = _inputVector.Normalized();
        }

        private void HandleWalking()
        {
            var isWalking = !_inputVector.IsEqual(Vector2.zero);
            if (isWalking)
            {
                _resetSmoothing = Vector3.zero;
                PlayWalkSequence();
            }
            else
            {
                _walkSequence.Pause();
                ResetModelRootPosition();
            }
        }

        public void PlayWalkSequence()
        {
            if (_walkSequence == null)
                CreateWalkSequence();

            if (_walkSequence.IsPlaying() && !_walkSequence.IsComplete())
                return;

            _walkSequence.Restart();
        }

        public void ResetModelRootPosition()
        {
            _modelRootLocalPosition.x = Mathf.SmoothDamp(transform.localPosition.x, _initialLocalPosition.x, ref _resetSmoothing.x, _walkResetDuration, Mathf.Infinity, Time.deltaTime);
            _modelRootLocalPosition.y = Mathf.SmoothDamp(transform.localPosition.y, _initialLocalPosition.y, ref _resetSmoothing.y, _walkResetDuration, Mathf.Infinity, Time.deltaTime);
            _modelRootLocalPosition.z = Mathf.SmoothDamp(transform.localPosition.z, _initialLocalPosition.z, ref _resetSmoothing.z, _walkResetDuration, Mathf.Infinity, Time.deltaTime);

            transform.localPosition = _modelRootLocalPosition;
        }

        private void CreateWalkSequence()
        {
            _walkSequence = DOTween.Sequence();
            _walkSequence.SetAutoKill(false);
            _walkSequence.Pause();

            _walkSequence.Append(transform.DOLocalJump(_rightLocalPosition, _jumpPower, 1, _jumpDuration).SetEase(_walkEase));
            _walkSequence.Append(transform.DOLocalJump(_initialLocalPosition, -_jumpPower, 1, _jumpDuration).SetEase(_walkEase));
            _walkSequence.Append(transform.DOLocalJump(_leftLocalPosition, _jumpPower, 1, _jumpDuration).SetEase(_walkEase));
            _walkSequence.Append(transform.DOLocalJump(_initialLocalPosition, -_jumpPower, 1, _jumpDuration).SetEase(_walkEase));
        }
    }
}
