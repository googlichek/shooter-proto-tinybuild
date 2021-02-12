using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class PlayerSoundController : TickComponent
    {
        [SerializeField] private List<AudioSource> _audioSources = new List<AudioSource>();

        [Space]

        [SerializeField] private List<AudioClip> _stepSounds = new List<AudioClip>();

        [Space]

        [SerializeField] private List<AudioClip> _shootSounds = new List<AudioClip>();

        private int _lastStepClipIndex;
        private int _lastAudioSourceIndex;

        public override void Enable()
        {
            _lastStepClipIndex = -1;
            _lastAudioSourceIndex = -1;
    }

        public void PlayStepSound()
        {
            var audioSource = GetAudioSource();
            var audioClip = GetStepAudioClip();

            if (audioClip == null)
                return;

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        
        public void PlayShootSound()
        {
            var audioSource = GetAudioSource();
            var audioClip = GetShootAudioClip();

            if (audioClip == null)
                return;

            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private AudioSource GetAudioSource()
        {
            for (var i = 0; i < _audioSources.Count; i++)
            {
                if (_audioSources[i].isPlaying)
                    continue;

                _lastAudioSourceIndex = i;
                return _audioSources[i];
            }

            var nextAudioSourceIndex = _lastAudioSourceIndex;
            if (_lastAudioSourceIndex != _audioSources.Count - 1)
                nextAudioSourceIndex++;
            else
                nextAudioSourceIndex = 0;

            _lastAudioSourceIndex = nextAudioSourceIndex;
            return _audioSources[nextAudioSourceIndex];
        }

        private AudioClip GetStepAudioClip()
        {
            var nextStepClipIndex = _lastStepClipIndex;
            if (_lastStepClipIndex != _stepSounds.Count - 1)
                nextStepClipIndex++;
            else
                nextStepClipIndex = 0;

            _lastStepClipIndex = nextStepClipIndex;
            return _stepSounds.Count > nextStepClipIndex
                ? _stepSounds[nextStepClipIndex]
                : null;
        }

        private AudioClip GetShootAudioClip()
        {
            if (_shootSounds.Count <= 0)
                return null;

            var nextShootClipIndex = Random.Range(0, _shootSounds.Count);
            return _shootSounds[nextShootClipIndex];
        }
    }
}
