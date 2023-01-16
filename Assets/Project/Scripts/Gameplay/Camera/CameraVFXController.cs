using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.VFX;
using Chaos.Gameplay.Characters;
using UnityEngine.Rendering;

namespace Chaos.Gameplay.Camera
{
    public class CameraVFXController : MonoBehaviour
    {
        public CharacterAnimationController CharacterAnimationControllerTest;
        public ScreenVolumeVisualEffect ScreenVFXTest;
        public float VolumeWeightUpdateTicksPerSeconds = 30f;

        private Volume _volumeComponent;
        private WaitForSeconds _coroutineWait;
        private IEnumerator _currentCoroutine;
        private UnityEngine.Camera _cachedCamera;
        private float _currentCamFoV;

        void Start()
        {
            Initialize();
        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S) == true)
            {
                ApplyScreenVisualEffectWithTween(ScreenVFXTest);
            }

            SetVolumeWeight(CharacterAnimationControllerTest.Animator.GetFloat("AttackIntensity"));
            
            SetCameraFoV(_currentCamFoV - CharacterAnimationControllerTest.Animator.GetFloat("AttackIntensity")*8f);
        }

        private void SetVolumeWeight(float value)
        {
            _volumeComponent.weight = value;
        }

        private void SetCameraFoV(float value)
        {
            _cachedCamera.fieldOfView = value;
        }
        public void Initialize()
        {
            _cachedCamera = UnityEngine.Camera.main;
            _volumeComponent = GetComponentInChildren<Volume>();
            if (VolumeWeightUpdateTicksPerSeconds > 0)
            {
                _coroutineWait = new WaitForSeconds(1f / VolumeWeightUpdateTicksPerSeconds);
            } else
            {
                _coroutineWait = new WaitForSeconds(1f / 30f);
            }

            _volumeComponent.profile = ScreenVFXTest.VolumeProfile;
            SetVolumeWeight(0f);
            _currentCamFoV = _cachedCamera.fieldOfView;
        }

        public void ChangeCameraVolumeProfile(VolumeProfile targetVolume)
        {
            _volumeComponent.profile = targetVolume;
        }

        public void ApplyScreenVisualEffectWithTween(ScreenVolumeVisualEffect targetScreenVisualEffect, float durationMultiplier = 1f)
        {
            
            _volumeComponent.profile = targetScreenVisualEffect.VolumeProfile;
            _volumeComponent.weight = Mathf.Clamp(targetScreenVisualEffect.EaseInAndOutCurve.Evaluate(0), 0f, 1f);

            //_coroutineWait = new WaitForSeconds((1f / VolumeWeightUpdateTicksPerSeconds) * durationMultiplier);

            StartCoroutine(TweenVolumeWeightByCurveRoutine(targetScreenVisualEffect.EaseInAndOutCurve, durationMultiplier));
        }

        private IEnumerator TweenVolumeWeightByCurveRoutine(AnimationCurve animationCurve, float durationMultiplier = 1f)
        {
            WaitForSeconds wait = new WaitForSeconds(durationMultiplier/30f);
            float incrementAmount = 1f/VolumeWeightUpdateTicksPerSeconds;
            float progress = 0f;
            while(progress <1f)
            {
                _volumeComponent.weight = Mathf.Clamp(animationCurve.Evaluate(progress), 0f, 1f);

                progress += incrementAmount;
                yield return wait;
            }

            _volumeComponent.weight = 0f;
        }
    }
}