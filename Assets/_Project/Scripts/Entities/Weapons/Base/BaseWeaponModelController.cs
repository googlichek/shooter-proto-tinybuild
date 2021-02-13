using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class BaseWeaponModelController : TickComponent, IWeaponModel
    {
        [SerializeField] private GameObject _model = default;

        [Space]

        [SerializeField] [Range(0, 1)] protected float switchInDuration = 0;
        [SerializeField] [Range(0, 1)] protected float switchOutDuration = 0;
        [SerializeField] [Range(0, 1)] protected float useInDuration = 0;
        [SerializeField] [Range(0, 1)] protected float useOutDuration = 0;

        [Space]

        [SerializeField] [Range(0, 1)] protected float switchOffsetX = 0;
        [SerializeField] [Range(0, 1)] protected float switchOffsetY = 0;
        [SerializeField] [Range(0, 1)] protected float useOffsetY = 0;
        [SerializeField] [Range(0, 1)] protected float useOffsetZ = 0;

        [Space]

        [SerializeField] [Range(0, 90)] protected float useOffsetRotation = 0;

        [Space]

        [SerializeField] protected Ease switchInEase = Ease.Linear;
        [SerializeField] protected Ease switchOutEase = Ease.Linear;
        [SerializeField] protected Ease useInEase = Ease.Linear;
        [SerializeField] protected Ease useOutEase = Ease.Linear;

        protected Sequence switchInSequence;
        protected Sequence switchOutSequence;
        protected Sequence useSequence;

        protected Vector3 initialPosition;
        protected Vector3 switchPosition;
        protected Vector3 usePosition;
        protected Vector3 useRotation;

        protected bool isUseAllowed;

        public bool IsUseAllowed => isUseAllowed;

        public override void Init()
        {
            initialPosition = transform.localPosition;
            switchPosition = transform.localPosition + new Vector3(switchOffsetX, -switchOffsetY, 0);
            usePosition = transform.localPosition + new Vector3(0, useOffsetY, -useOffsetZ);
            useRotation = transform.localRotation.eulerAngles + new Vector3(-useOffsetRotation, 0, 0);
        }

        public virtual void SwitchIn()
        {
            if (switchOutSequence != null && switchOutSequence.IsPlaying() && !switchOutSequence.IsComplete())
                switchOutSequence.Pause();

            PlaySwitchInSequence();
        }

        public virtual void SwitchOut()
        {
            if (switchInSequence != null && switchInSequence.IsPlaying() && !switchInSequence.IsComplete())
                switchInSequence.Pause();

            PlaySwitchOutSequence();
        }

        public virtual void Use()
        {
            if (switchOutSequence != null && switchOutSequence.IsPlaying() && !switchOutSequence.IsComplete())
                switchOutSequence.Complete();

            if (switchInSequence != null && switchInSequence.IsPlaying() && !switchInSequence.IsComplete())
                switchInSequence.Complete();

            PlayUseSequence();
        }

        protected virtual void PlaySwitchInSequence()
        {
            if (switchInSequence == null)
                CreateSwitchInSequence();

            switchInSequence.Restart();
        }

        protected virtual void PlaySwitchOutSequence()
        {
            if (switchOutSequence == null)
                CreateSwitchOutSequence();

            switchOutSequence.Restart();
        }

        protected virtual void PlayUseSequence()
        {
            if (useSequence == null)
                CreateUseSequence();

            useSequence.Restart();
        }

        protected virtual void CreateSwitchInSequence()
        {
            switchInSequence = DOTween.Sequence();
            switchInSequence.SetAutoKill(false);
            switchInSequence.Pause();

            switchInSequence.AppendCallback(() => _model.SetActive(true));

            switchInSequence.Append(transform.DOLocalMove(switchPosition, 0));
            switchInSequence.Append(transform.DOLocalMove(initialPosition, switchInDuration).SetEase(switchInEase));

            switchInSequence.SetDelay(switchOutDuration);

            switchInSequence.AppendCallback(() => isUseAllowed = true);
        }

        protected virtual void CreateSwitchOutSequence()
        {
            switchOutSequence = DOTween.Sequence();
            switchOutSequence.SetAutoKill(false);
            switchOutSequence.Pause();

            switchOutSequence.AppendCallback(() => isUseAllowed = false);

            switchOutSequence.Append(transform.DOLocalMove(initialPosition, 0));
            switchOutSequence.Append(transform.DOLocalMove(switchPosition, switchOutDuration).SetEase(switchOutEase));

            switchOutSequence.AppendCallback(() => _model.SetActive(false));
        }

        protected virtual void CreateUseSequence()
        {
            useSequence = DOTween.Sequence();
            useSequence.SetAutoKill(false);
            useSequence.Pause();

            useSequence.Append(transform.DOLocalMove(usePosition, useInDuration).SetEase(useInEase));
            useSequence.Join(transform.DOLocalRotate(useRotation, useInDuration).SetEase(useInEase));
            
            useSequence.Append(transform.DOLocalMove(initialPosition, useOutDuration).SetEase(useOutEase));
            useSequence.Join(transform.DOLocalRotate(Vector3.zero, useOutDuration).SetEase(useOutEase));
        }
    }
}
