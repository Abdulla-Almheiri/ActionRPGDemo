using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.UI
{
    public class FloatingCombatTextAnimationController : MonoBehaviour
    {
        public FloatingCombatTextTemplate FloatingCombatTextTemplate;
        private GameObjectPoolController _gameObjectPoolController;
        private TMP_Text _TMPText;
        private Vector3 _currentPosition;
        private float _elapsedTime = 0f;
        private float _maxDurationInSeconds = 2f;
        private bool _followTransform = false;
        private Vector3 _initialPosition;
        private UnityEngine.Camera _cachedCamera;

        public void Awake()
        {
            _cachedCamera = UnityEngine.Camera.main;
            _TMPText = GetComponent<TMP_Text>();
            _gameObjectPoolController = GetComponent<GameObjectPoolController>();
            
        }

        void Start()
        {
            //Initialize(new Vector3(400f,400f,0f));
        }

        void Update()
        {
            ProcessDisablingAfterElapsedTime();
            ProcessFollowingInitializedTransform();
        }
        private void ProcessDisablingAfterElapsedTime()
        {
            if(gameObject.activeSelf == false)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if(_elapsedTime >= _maxDurationInSeconds)
            {
                gameObject.SetActive(false);
            }
        }

        private void ProcessFollowingInitializedTransform()
        {
            if(_initialPosition == null || _followTransform == false)
            {
                return;
            }
            //HERE FIX
            _currentPosition = _cachedCamera.WorldToScreenPoint(_initialPosition);
            _currentPosition.z = 0f;
        }

        public void Initialize(Vector3 initialPosition)
        {
            _TMPText = GetComponent<TMP_Text>();
            _currentPosition = initialPosition;
            _TMPText.rectTransform.SetPositionAndRotation(_currentPosition, Quaternion.identity);
            Play();
        }


        public void Initialize(Transform worldPoint, FloatingCombatTextTemplate floatingCombatTextTemplate, bool followTransform = false)
        {
            FloatingCombatTextTemplate = floatingCombatTextTemplate;
            _followTransform = followTransform;
        }

        public void Play()
        {
            if(FloatingCombatTextTemplate == null || _TMPText == null)
            {
                return;
            }
            foreach(FloatingCombatTextAnimationData animationData in FloatingCombatTextTemplate.PropertiesToAnimate)
            {
                StartCoroutine(AnimatePropertyWithAnimationCurveCO(animationData));
            }

        }


        public void Play(Transform worldPoint, FloatingCombatTextTemplate floatingCombatTextTemplate, bool followTransform = false)
        {
            if ( _TMPText == null)
            {
                return;
            }
            _initialPosition = worldPoint.position;
            _followTransform = followTransform;

            var initialPosition = _cachedCamera.WorldToScreenPoint(worldPoint.position);
           initialPosition.z = 0f;

            float xVariation = FloatingCombatTextTemplate.StartingPositionVariationAmount.x;
            float yVariation = FloatingCombatTextTemplate.StartingPositionVariationAmount.y;
            initialPosition.x +=  Random.Range(-xVariation, xVariation);
            initialPosition.y += Random.Range(-yVariation, yVariation);

           _TMPText.rectTransform.SetPositionAndRotation(initialPosition, Quaternion.identity);

            _currentPosition = initialPosition;

            foreach (FloatingCombatTextAnimationData animationData in floatingCombatTextTemplate.PropertiesToAnimate)
            {
                StartCoroutine(AnimatePropertyWithAnimationCurveCO(animationData));
            }
        }


        private IEnumerator AnimatePropertyWithAnimationCurveCO(FloatingCombatTextAnimationData animationData)
        {
            float increment = Time.deltaTime;

            var wait = new WaitForEndOfFrame();
            float progress = 0f;
            while(progress <= 1f+increment)
            {
                increment = Time.deltaTime;
                SetPropertyValue(animationData.PropertyToAnimate, animationData.CurveMultiplier * animationData.AnimationCurve.Evaluate(progress));
                progress += increment;
                yield return wait;
            }
        }

        private void SetPropertyValue(FloatingCombatTextAnimationDataPropertyType propertyType, float newValue)
        {

            if(propertyType == FloatingCombatTextAnimationDataPropertyType.RelativePositionX)
            {
                Vector3 newPosition = _currentPosition + (new Vector3(newValue, 0f, 0f));
                _TMPText.rectTransform.SetPositionAndRotation(newPosition, Quaternion.identity);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.RelativePositionY)
            {
                Vector3 newPosition = _currentPosition + (new Vector3(0f, newValue, 0f));
                _TMPText.rectTransform.SetPositionAndRotation(newPosition, Quaternion.identity);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.Scale)
            {
                _TMPText.transform.localScale = new Vector3(newValue, newValue, newValue);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.Transparency)
            {
                var oldColor = _TMPText.color;
                var newColor = oldColor;
                newColor.a = newValue;
                _TMPText.color = newColor;
                return;
            }
        }


        public void OnEnable()
        {
            
            //Play();
            ResetElapsedTime();
        }


        private void ResetElapsedTime()
        {
            _elapsedTime = 0f;
        }
        public void OnDisable()
        {
            //_gameObjectPoolController.ReturnToPool();
        }

    }
}