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

        [Space]

        [SerializeField] [Range(0, 1)] protected float switchOffsetX = 0;
        [SerializeField] [Range(0, 1)] protected float switchOffsetY = 0;

        [Space]

        [SerializeField] protected Ease switchInEase = Ease.Linear;
        [SerializeField] protected Ease switchOutEase = Ease.Linear;

        protected Sequence switchInSequence;
        protected Sequence switchOutSequence;

        protected Vector3 initialPosition;
        protected Vector3 switchPosition;

        public override void Init()
        {
            initialPosition = transform.localPosition;
            switchPosition = transform.localPosition + new Vector3(switchOffsetX, -switchOffsetY, 0);
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

        protected virtual void CreateSwitchInSequence()
        {
            switchInSequence = DOTween.Sequence();
            switchInSequence.SetAutoKill(false);
            switchInSequence.Pause();

            switchInSequence.AppendCallback(() => gameObject.SetActive(true));

            switchInSequence.Append(transform.DOLocalMove(switchPosition, 0));
            switchInSequence.Append(transform.DOLocalMove(initialPosition, switchInDuration).SetEase(switchInEase));

            switchInSequence.SetDelay(switchOutDuration);
        }

        protected virtual void CreateSwitchOutSequence()
        {
            switchOutSequence = DOTween.Sequence();
            switchOutSequence.SetAutoKill(false);
            switchOutSequence.Pause();

            switchOutSequence.Append(transform.DOLocalMove(initialPosition, 0));
            switchOutSequence.Append(transform.DOLocalMove(switchPosition, switchOutDuration).SetEase(switchOutEase));

            switchOutSequence.AppendCallback(() => gameObject.SetActive(false));
        }
    }
}
